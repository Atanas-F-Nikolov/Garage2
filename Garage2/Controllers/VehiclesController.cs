using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Garage2.DAL;
using Garage2.Models;

namespace Garage2.Controllers
{
    public class VehiclesController : Controller
    {
        private Garage2Context db = new Garage2Context();

        // GET: Vehicles
        public ActionResult Index()
        {
            var vehicles = db.Vehicles;
            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Type");
            return View(vehicles.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string regNr, int? VehicleTypeId)
        {
            var vehicles = db.Vehicles.ToList();
            vehicles = SearchInList(regNr, VehicleTypeId, vehicles);
            return View(vehicles);
        }

        private List<Vehicle> SearchInList(string regNr, int? VehicleTypeId, List<Vehicle> vehicles)
        {
            vehicles = vehicles
                .Where(x => (!string.IsNullOrWhiteSpace(regNr)) ? x.RegNumber.Equals(regNr) : true)
                .Where(x => (VehicleTypeId != null) ? x.Type.Id.Equals(VehicleTypeId) : true).ToList();
            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Type");
            return vehicles;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BasicOverView(string regNr, int? VehicleTypeId)
        {
            List<VehicleDetailsViewModel> list = CreateViewModelList();
            return View(list);
        }

        public ActionResult BasicOverView()
        {
            List<VehicleDetailsViewModel> list = CreateViewModelList();

            return View(list);
        }

        private List<VehicleDetailsViewModel> CreateViewModelList()
        {
            var list = db.Vehicles.Select(x =>
                        new VehicleDetailsViewModel
                        {
                            VehicleId = x.Id,
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


        // GET: Vehicles/Create
        public ActionResult Create()
        {
            ViewBag.MemberId = new SelectList(db.Members, "Id", "FullName");
            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Type");
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ParkingSpace,Size,RegNumber,Color,Brand,Model,Wheels,CheckInTimeStamp,VehicleTypeId,MemberId")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                db.Vehicles.Add(vehicle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MemberId = new SelectList(db.Members, "Id", "FullName", vehicle.MemberId);
            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Type", vehicle.VehicleTypeId);
            return View(vehicle);
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
