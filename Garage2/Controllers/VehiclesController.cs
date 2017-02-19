﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Web.Mvc;
using Garage2.DAL;
using Garage2.Models;

namespace Garage2.Controllers
{
    public class VehiclesController : Controller
    {
        private Garage2Context db = new Garage2Context();
        private int garageSize = 30;
        private string motorCycle = "Motorcycle";

        // GET: Vehicles
        public ActionResult Index(string msg, bool hasAddedVehicle = false, string title = "List of vehicles", string sort = "", [Bind(Include = "RegNr,CheckIn,Color,Brand,Model,Wheels,Owner,VehicleTypeId")] VehicleDetailSearchParams searchParams = null)
        {
            List<Vehicle> vehicles = db.Vehicles.ToList();

            if (searchParams != null)
            {
                vehicles = SearchInList(searchParams, vehicles);
            }
            else
            {
                searchParams = new VehicleDetailSearchParams();
            }

            vehicles = SortList(ref sort, vehicles);

            return View(new VehicleIndexViewModel
            {
                SearchParams = searchParams,
                Sort = sort,
                VehicleTypes = new SelectList(db.VehicleTypes, "Id", "Type"),
                Title = title,
                Vehicles = vehicles.ToList(),
                HasAddedVehicle = hasAddedVehicle,
                Message = msg
            });
        }

        private List<Vehicle> SortList(ref string sort, List<Vehicle> vehicleList)
        {
            if (!string.IsNullOrWhiteSpace(sort))
            {
                sort = sort.Replace("_", " ");

                if (sort.Contains("Owner")) vehicleList = (sort.Contains("descending") ? vehicleList.OrderByDescending(x => x.Owner.FullName).ToList() : vehicleList.OrderBy(x => x.Owner.FullName).ToList());
                else if (sort.Contains("Size")) vehicleList = (sort.Contains("descending") ? vehicleList.OrderByDescending(x => x.Type.Size).ToList() : vehicleList.OrderBy(x => x.Type.Size).ToList());
                else if (sort.Contains("Type")) vehicleList = (sort.Contains("descending") ? vehicleList.OrderByDescending(x => x.Type.Type).ToList() : vehicleList.OrderBy(x => x.Type.Type).ToList());
                else vehicleList = vehicleList.OrderBy(sort).ToList();

                sort = (sort.Contains("descending") ? sort.Substring(0, sort.IndexOf(" ")) : sort + "_descending");
                return vehicleList;
            }
            else
            {
                return vehicleList.OrderByDescending(x => x.Id).ToList();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(VehicleDetailSearchParams searchParams, string title = "List of vehicles")
        {
            var vehicles = db.Vehicles.ToList();
            string sort = "";
            vehicles = SortList(ref sort, vehicles);

            if (Request.Form["show"] != null) ModelState.Clear();
            else vehicles = SearchInList(searchParams, vehicles);

            return View(new VehicleIndexViewModel { SearchParams = (searchParams != null) ? searchParams : new VehicleDetailSearchParams(), Sort = "", VehicleTypes = new SelectList(db.VehicleTypes, "Id", "Type"), Vehicles = vehicles, Title = title });
        }

        private static List<Vehicle> SearchInList(VehicleDetailSearchParams searchParams, List<Vehicle> vehicles)
        {
            vehicles = vehicles
                       .Where(x => (!string.IsNullOrWhiteSpace(searchParams.RegNr)) ? x.RegNumber.ToLower().Contains(searchParams.RegNr.ToLower()) : true)
                       .Where(x => (!string.IsNullOrWhiteSpace(searchParams.Owner)) ? x.Owner.FullName.ToLower().Contains(searchParams.Owner.ToLower()) : true)
                       .Where(x => (!string.IsNullOrWhiteSpace(searchParams.Model) && x.Model != null) ? x.Model.ToLower().Contains(searchParams.Model.ToLower()) : true)
                       .Where(x => (!string.IsNullOrWhiteSpace(searchParams.Color) && x.Color != null) ? x.Color.ToLower().Contains(searchParams.Color.ToLower()) : true)
                       .Where(x => (!string.IsNullOrWhiteSpace(searchParams.Brand) && x.Brand != null) ? x.Brand.ToLower().Contains(searchParams.Brand.ToLower()) : true)
                       .Where(x => (searchParams.VehicleTypeId != null) ? x.Type.Id.Equals(searchParams.VehicleTypeId) : true)
                       .Where(x => (searchParams.Wheels != null) ? x.Wheels.Equals(searchParams.Wheels) : true)
                       .Where(x => (searchParams.CheckIn != null) ? x.CheckInTimeStamp.Date.Equals(searchParams.CheckIn.Value.Date) : true)
                       .ToList();
            return vehicles;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BasicOverView(string regNr, int? VehicleTypeId)
        {
            List<VehicleDetailsViewModel> list = CreateViewModelList();
            if (Request.Form["show"] != null)
            {
                ModelState.Clear();
            }
            else
            {
                list = list
                       .Where(x => (!string.IsNullOrWhiteSpace(regNr)) ? x.RegNumber.ToLower().Contains(regNr.ToLower()) : true)
                       .Where(x => (VehicleTypeId != null) ? x.TypeId.Equals(VehicleTypeId) : true).ToList();
            }
            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Type");
            return View(list);
        }

        public ActionResult BasicOverView()
        {
            List<VehicleDetailsViewModel> list = CreateViewModelList();
            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Type");
            return View(list);
        }

        private List<VehicleDetailsViewModel> CreateViewModelList()
        {
            var list = db.Vehicles.Select(x =>
                        new VehicleDetailsViewModel
                        {
                            VehicleId = x.Id,
                            TypeId = x.Type.Id,
                            Owner = x.Owner.FirstName + " " + x.Owner.LastName,
                            ParkingTime = x.CheckInTimeStamp.ToString(),
                            RegNumber = x.RegNumber,
                            Type = x.Type.Type
                        }
                        ).ToList();

            foreach (var item in list)
            {
                var parkingString = "";

                var checkIn = DateTime.Parse(item.ParkingTime);
                var now = DateTime.Now;

                var timeSpan = now.Subtract(checkIn);

                var days = timeSpan.Days;
                if (days > 0) parkingString += $"Days: {days}, ";

                var hours = timeSpan.Hours;
                if (hours > 0) parkingString += $"Hours: {hours}, ";

                var minutes = timeSpan.Minutes;
                if (minutes > 0) parkingString += $"Minutes: {minutes} ";

                item.ParkingTime = parkingString;
            }

            return list;
        }

        // GET: Vehicles/Details/5
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

        // GET: Vehicles/Create
        public ActionResult Create()
        {
            return View(new VehicleCreateViewModel
            {
                VehicleTypes = new SelectList(db.VehicleTypes, "Id", "Type"),
                Owners = new SelectList(db.Members, "Id", "FullName")
            });
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Type,RegNumber,Color,Brand,Model,Wheels,VehicleTypeId,MemberId")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                int parking = -1;
                var type = db.VehicleTypes.Find(vehicle.VehicleTypeId);
                if (type.Type == motorCycle) parking = GetParkingSpaceForMotorCycle();
                else parking = GetParkingSpace(type.Size);

                if (parking > 0)
                {
                    vehicle.ParkingSpace = parking;
                    db.Vehicles.Add(vehicle);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { hasAddedVehicle = true, createdVehicleId = vehicle.Id, msg = $"Your {type.Type} has been parked successfully" });
                }
                else ModelState.AddModelError("", $"There is no space in the garage for your {type}");
            }

            return View(new VehicleCreateViewModel
            {
                Vehicle = vehicle,
                Spaces = GetFreeSpaces(),
                VehicleTypes = new SelectList(db.VehicleTypes, "Id", "Type", vehicle.VehicleTypeId),
                Owners = new SelectList(db.Members, "Id", "FullName", vehicle.MemberId)
            });
        }

        private int GetParkingSpace(int size)
        {
            var spaces = db.Vehicles.Select(x => new
            {
                space = x.ParkingSpace,
                size = x.Type.Size
            }).Distinct().OrderBy(x => x.space).ToList();

            var currentSpace = 1;

            int[] requiredSpaces = new int[size];
            int[] testSpaces;
            foreach (var item in spaces)
            {
                testSpaces = new int[item.size];

                for (int i = 0; i < size; i++) requiredSpaces[i] = currentSpace + i;
                for (int i = 0; i < item.size; i++) testSpaces[i] = item.space + i;

                if (requiredSpaces.Any(x => testSpaces.Contains(x))) currentSpace = (item.space + item.size);
                else break;
            }

            if (currentSpace + (size - 1) > garageSize) currentSpace = -1;

            return currentSpace;
        }

        private int GetParkingSpaceForMotorCycle()
        {
            var spaces = db.Vehicles.Where(x => x.Type.Type == motorCycle)
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

        private FreeSpaces GetFreeSpaces()
        {
            var freeSpacesDetails = new FreeSpaces();
            freeSpacesDetails.AllSpaces = garageSize;

            var freeSpaces = 0;

            var spaces = db.Vehicles
                .Select(x => new
                {
                    space = x.ParkingSpace,
                    size = x.Type.Size
                }).Distinct().ToList();

            var count = garageSize;
            foreach (var item in spaces) count -= item.size;

            if (count < 0) { freeSpacesDetails.RegularSpaces = 0; }
            else { freeSpacesDetails.RegularSpaces = count; }

            freeSpaces = count;
            count = 0;

            var motorcycleSpaces = db.Vehicles.Where(x => x.Type.Type == motorCycle)
                .GroupBy(g => g.ParkingSpace)
                .Select(y => new { Space = y.Key, Count = y.Count() }).ToList();

            foreach (var item in motorcycleSpaces)
            {
                if (item.Count < 3) count += (3 - item.Count);
            }

            freeSpaces += count;
            freeSpacesDetails.MotorSpaces = count;

            return freeSpacesDetails;
        }

        // GET: Vehicles/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.MemberId = new SelectList(db.Members, "Id", "FirstName", vehicle.MemberId);
            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Type", vehicle.VehicleTypeId);
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ParkingSpace,Size,RegNumber,Color,Brand,Model,Wheels,CheckInTimeStamp,VehicleTypeId,MemberId")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehicle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MemberId = new SelectList(db.Members, "Id", "FirstName", vehicle.MemberId);
            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Type", vehicle.VehicleTypeId);
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
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

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);
            db.Vehicles.Remove(vehicle);
            db.SaveChanges();
            return RedirectToAction("Index");
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
