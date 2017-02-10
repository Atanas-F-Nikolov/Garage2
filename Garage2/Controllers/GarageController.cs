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

        // GET: Garage
        public ActionResult Index(DateTime? time, VehicleType? type,
            int? wheels, string sort, string reg,
            string color, string brand, string model, string msg)
        {

            ViewBag.Message = (string.IsNullOrWhiteSpace(msg)) ? "List of vehicles" : msg;
            List<Vehicle> list = db.Vehicles.ToList();

            if (Request.Form["search"] != null)
            {
                ViewBag.reg = reg;
                ViewBag.type = type;
                ViewBag.color = color;
                ViewBag.brand = brand;
                ViewBag.model = model;
                ViewBag.wheels = wheels;
                ViewBag.time = time;

                list = list
                    .Where(x => (!string.IsNullOrWhiteSpace(reg)) ? x.RegNumber.ToLower().Contains(reg.ToLower()) : true)
                    .Where(x => (time != null) ? x.TimeStamp.Date.Equals(time.Value.Date) : true)
                    .Where(x => (type != null) ? x.Type.Equals(type) : true)
                    .Where(x => (wheels != null) ? x.Wheels.Equals(wheels) : true)
                    .Where(x => (!string.IsNullOrWhiteSpace(color)) ? x.Color.ToLower().Equals(color.ToLower()) : true)
                    .Where(x => (!string.IsNullOrWhiteSpace(brand)) ? x.Brand.ToLower().Equals(brand.ToLower()) : true)
                    .Where(x => (!string.IsNullOrWhiteSpace(model)) ? x.Model.ToLower().Equals(model.ToLower()) : true)
                    .ToList();
            }
            else
            {
                ModelState.Clear();
            }

            if (!string.IsNullOrWhiteSpace(sort)) { list = list.OrderBy(sort).ToList(); }
            else { list = list.OrderByDescending(x => x.Id).ToList(); }
            return View(list);
        }

        public ActionResult CheckOut(DateTime? time, VehicleType? type,
            int? wheels, string sort, string reg,
            string color, string brand, string model)
        {
            return RedirectToActionPermanent("Index", new {
                time = time, type = type, wheels = wheels,
                sort = sort, reg = reg, color = color,
                brand = brand, model = model, msg = "Select vehicle to check-out"
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
        public ActionResult Receipt(int id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);
            //db.Vehicles.Remove(vehicle);
            //db.SaveChanges();
            return View("Receipt", vehicle);
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
