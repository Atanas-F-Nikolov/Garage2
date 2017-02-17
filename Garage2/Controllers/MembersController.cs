﻿using System;
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
    public class MembersController : Controller
    {
        private Garage2Context db = new Garage2Context();

        // GET: Members
        public ActionResult OverView()
        {
            return View(db.Members.ToList());
        }

        public ActionResult DoesUserExist(string SocialSecurityNumber) => Json(!db.Members.Any(x => x.SocialSecurityNumber.Equals(SocialSecurityNumber)), JsonRequestBehavior.AllowGet);
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OverView(string socialNr, string fName, string lName, DateTime? birth)
        {
            var list = db.Members.ToList();

            list = list
                .Where(x => (!string.IsNullOrWhiteSpace(socialNr)) ? x.SocialSecurityNumber.ToLower().Contains(socialNr.ToLower()) : true)
                .Where(x => (!string.IsNullOrWhiteSpace(fName)) ? x.FirstName.ToLower().Contains(fName.ToLower()) : true)
                .Where(x => (!string.IsNullOrWhiteSpace(lName)) ? x.LastName.ToLower().Contains(lName.ToLower()) : true)
                .Where(x => (birth != null) ? x.DateOfBirth.Date.Equals(birth.Value.Date) : true)
                .ToList();

            return View(list);
        }

        // GET: Members/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // GET: Members/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SocialSecurityNumber,FirstName,LastName,DateOfBirth")] Member member)
        {
            if (ModelState.IsValid)
            {
                db.Members.Add(member);
                db.SaveChanges();
                return RedirectToAction("OverView");
            }

            return View(member);
        }

        // GET: Members/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SocialSecurityNumber,FirstName,LastName,DateOfBirth")] Member member)
        {
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("OverView");
            }
            return View(member);
        }

        // GET: Members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member member = db.Members.Find(id);
            db.Members.Remove(member);
            db.SaveChanges();
            return RedirectToAction("OverView");
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
