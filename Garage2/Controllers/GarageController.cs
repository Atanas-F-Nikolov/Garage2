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
using System.Linq.Dynamic;

namespace Garage2.Controllers
{
    public class GarageController : Controller
    {
        private Garage2Context db = new Garage2Context();

        // GET: Garage
        public ActionResult Index(string sort, string reg,
                VehicleType? type, string color, string brand,
                string model, int? wheels, DateTime? time)
        {
            List<Vehicle> list = db.Vehicles.ToList();

            ViewBag.reg = reg;
            ViewBag.type = type;
            ViewBag.color = color;
            ViewBag.brand = brand;
            ViewBag.model = model;
            ViewBag.wheels = wheels;
            ViewBag.time = time;

            if (!string.IsNullOrEmpty(reg)) list = list.Where("RegNumber.ToLower().Contains(@0)", reg.ToLower()).ToList();
            if (type != null) list = list.Where("Type == (@0)", type).ToList();
            if (!string.IsNullOrEmpty(color)) list = list.Where("Color == (@0)", color).ToList();
            if (!string.IsNullOrEmpty(brand)) list = list.Where("Brand == (@0)", brand).ToList();
            if (!string.IsNullOrEmpty(model)) list = list.Where("Model == (@0)", model).ToList();
            if (wheels != null) list = list.Where("Wheels == (@0)", wheels).ToList();
            if (time != null) list = list.Where("TimeStamp == (@0)", time).ToList();

            if (!string.IsNullOrWhiteSpace(sort))
            {
                list = list.OrderBy(sort).ToList();
            }
            else
            {
                list = list.OrderByDescending(x => x.Id).ToList();
            }
            return View(list);
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

        // POST: Garage/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Type,RegNumber,Color,Brand,Model,Wheels")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                db.Vehicles.Add(vehicle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

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
