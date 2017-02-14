using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Garage2.DAL;
using Garage2.Models;
using System.Linq.Dynamic;

namespace Garage2.Controllers
{
    public class GarageController : Controller
    {
        private Garage2Context db = new Garage2Context();
        private double pricePerHour = 100;

        // GET: Garage
        public ActionResult Index(DateTime? time, VehicleType? type,
            int? wheels, string sort, string reg,
            string color, string brand, string model, string msg = "")
        {

            ViewBag.Message = (string.IsNullOrWhiteSpace(msg)) ? "List of vehicles" : msg;

            List<Vehicle> list = db.Vehicles.ToList();

            if (Request.Form["show"] != null)
            {
                ModelState.Clear();
                if (!string.IsNullOrWhiteSpace(sort))
                {
                    if (sort.Contains("descending"))
                    {
                        sort = sort.Replace("_descending", "");
                        ViewBag.sort = sort;
                    }
                    else
                    {
                        sort = sort + " " + "descending";
                        ViewBag.sort = sort;
                    }
                }
                else { list = list.OrderByDescending(x => x.Id).ToList(); }
                return View(list);
            }

            ViewBag.msg = msg;
            ViewBag.reg = reg;
            ViewBag.type = type;
            ViewBag.color = color;
            ViewBag.brand = brand;
            ViewBag.model = model;
            ViewBag.wheels = wheels;
            ViewBag.time = time;

            list = list
                .Where(x => (!string.IsNullOrWhiteSpace(reg)) ? x.RegNumber.ToLower().Contains(reg.ToLower()) : true)
                .Where(x => (time != null) ? x.CheckInTimeStamp.Date.Equals(time.Value.Date) : true)
                .Where(x => (type != null) ? x.Type.Equals(type) : true)
                .Where(x => (wheels != null) ? x.Wheels.Equals(wheels) : true)
                .Where(x => (!string.IsNullOrWhiteSpace(color)) ? x.Color.ToLower().Equals(color.ToLower()) : true)
                .Where(x => (!string.IsNullOrWhiteSpace(brand)) ? x.Brand.ToLower().Equals(brand.ToLower()) : true)
                .Where(x => (!string.IsNullOrWhiteSpace(model)) ? x.Model.ToLower().Equals(model.ToLower()) : true)
                .ToList();

            if (!string.IsNullOrWhiteSpace(sort))
            {
                if (sort.Contains("descending"))
                {
                    sort = sort.Replace("_descending", "");
                    ViewBag.sort = sort;
                }
                else
                {
                    sort = sort + " " + "descending";
                    ViewBag.sort = sort;
                }
                list = list.OrderBy(sort).ToList();

            }
            else { list = list.OrderByDescending(x => x.Id).ToList(); }
            return View(list);
        }

        public ActionResult CheckOut(DateTime? time, VehicleType? type,
            int? wheels, string sort, string reg,
            string color, string brand, string model)
        {
            return RedirectToActionPermanent("Index", new
            {
                time = time,
                type = type,
                wheels = wheels,
                sort = sort,
                reg = reg,
                color = color,
                brand = brand,
                model = model,
                msg = "Select vehicle to check-out"
            });
        }

        // GET: Garage/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        // GET: Garage/Create
        public ActionResult Create()
        {
            return View();
        }

        private int garageSize = 30;

        private int GetParkingSpaceForMotorCycle()
        {
            var spaces = db.Vehicles.Where(x => x.Type == VehicleType.Motorcycle)
                .GroupBy(g => g.ParkingSpace)
                .Select(y => new { Space = y.Key, Count = y.Count() }).ToList();

            foreach (var parkingSpace in spaces)
            {
                if (parkingSpace.Count < 3)
                {
                    return parkingSpace.Space;
                }
            }
            return GetParkingSpace(1);
        }

        private int GetParkingSpace(int size)
        {
            var spaces = db.Vehicles.Select(x => new
            {
                space = x.ParkingSpace,
                size = x.Size
            }).Distinct().OrderBy(x => x.space).ToList();

            var currentSpace = 1;

            int[] requiredSpaces = new int[size];
            int[] testSpaces;
            foreach (var item in spaces)
            {
                for (int i = 0; i < size; i++)
                {
                    requiredSpaces[i] = currentSpace + i;
                }
                testSpaces = new int[item.size];
                for (int i = 0; i < item.size; i++)
                {
                    testSpaces[i] = item.space + i;
                }

                if (requiredSpaces.Any(x => testSpaces.Contains(x)))
                {
                    currentSpace = (item.space + item.size);
                    if (currentSpace > garageSize) return -1;
                }
                else break;
            }
            return currentSpace;
        }

        // POST: Garage/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Type,RegNumber,Color,Brand,Model,Wheels")] Vehicle vehicle)
        {
            List<string> regNumberslist = db.Vehicles.Select(x => x.RegNumber.ToLower()).ToList();
            if (!regNumberslist.Contains(vehicle.RegNumber.ToLower()))
            {
                if (ModelState.IsValid)
                {
                    switch (vehicle.Type)
                    {
                        case VehicleType.Airplane:
                            vehicle.Size = 3;
                            break;
                        case VehicleType.Boat:
                            vehicle.Size = 3;
                            break;
                        case VehicleType.Buss:
                            vehicle.Size = 2;
                            break;
                        case VehicleType.Car:
                            vehicle.Size = 1;
                            break;
                        case VehicleType.Motorcycle:
                            vehicle.Size = 1;
                            break;
                    }

                    var parking = (vehicle.Type == VehicleType.Motorcycle) ? GetParkingSpaceForMotorCycle() : GetParkingSpace(vehicle.Size);

                    if (parking != -1)
                    {
                        vehicle.ParkingSpace = parking;
                        db.Vehicles.Add(vehicle);
                        db.SaveChanges();
                        return RedirectToAction("Index", new { msg = $"List of vehicles - Your {vehicle.Type} has been parked successfully" });
                    }
                }
            }
            ViewBag.regNErrorMessage = "There is such a Registration Number in DB!";
            return View(vehicle);
        }

        // GET: Garage/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        // POST: Garage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Receipt(int id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);
            Receipt receipt = new Receipt();
            receipt.PricePerHour = pricePerHour;
            receipt.vehicle = vehicle;
            var parkingPeriod = receipt.CheckOutTimeStamp.Subtract(vehicle.CheckInTimeStamp);
            receipt.ParkingsPeriodInMin = Math.Round(parkingPeriod.TotalMinutes);
            receipt.TotalPrice = Math.Ceiling((receipt.PricePerHour / 60) * receipt.ParkingsPeriodInMin);

            db.Vehicles.Remove(vehicle);
            db.SaveChanges();

            return View("Receipt", receipt);
        }


        public ActionResult Statistics()
        {
            
            List<Vehicle> vehicleList = db.Vehicles.ToList();
            var StatistiscInAGroup = vehicleList
                .Where(v => v != null)
                .GroupBy(v => v.Type)
                .OrderBy(v => v.Key)
                .Select(t => new GroupByTypeStatistics
                {
                    VehiclesNumberInAGroup = t.Count(),
                    VehicleGroup = t.Key,
                    WheelsNumberInAGroup = t.Sum(x => x.Wheels),
                    ParkingTimeInAGroup = t.Sum(x => Math.Round(DateTime.Now.Subtract(x.CheckInTimeStamp).TotalMinutes)),
                    ParkingPriceInAGroup = t.Sum(x => Math.Ceiling((pricePerHour / 60) * Math.Round(DateTime.Now.Subtract(x.CheckInTimeStamp).TotalMinutes)))
                });

            var statistics = new Statistics();
            statistics.GroupByDiffStatistics = StatistiscInAGroup.ToList();
            
            foreach (var item in statistics.GroupByDiffStatistics)
            {
                statistics.TotalVehiclesNumber += item.VehiclesNumberInAGroup;
                statistics.TotalWheelsNumber += item.WheelsNumberInAGroup;
                statistics.TotalParkingTime += item.ParkingTimeInAGroup;
                statistics.TotalParkingPrice += item.ParkingPriceInAGroup;
            }

            ViewBag.PricePerHour = pricePerHour;
            return View(statistics);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
