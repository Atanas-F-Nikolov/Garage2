using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Garage2.DAL;
using Garage2.Models;
using System.Linq.Dynamic;
using System.Linq;

namespace Garage2.Controllers
{
    public class MembersController : Controller
    {
        private Garage2Context db = new Garage2Context();

        // GET: Members
        public ActionResult OverView(string msg, bool hasAddedMember = false, string sort = "", string title = "List of members", [Bind(Include = "SocialNr,FirstName,LastName,DateOfBirth")] MemberSearchParams searchParams = null)
        {
            List<Member> membersList = db.Members.ToList(); ;

            if (searchParams != null)
            {
                membersList = SearchInList(searchParams, membersList);
            }
            else
            {
                searchParams = new MemberSearchParams();
            }

            membersList = SortList(ref sort, membersList);

            return View(new MembersOverViewViewModel { Title = title, Members = membersList, Sort = sort, HasAddedMember = hasAddedMember, Message = msg, SearchParams = searchParams });
        }

        private List<Member> SortList(ref string sort, List<Member> membersList)
        {
            if (!string.IsNullOrWhiteSpace(sort))
            {
                sort = sort.Replace("_", " ");
                membersList = membersList.OrderBy(sort).ToList();
                sort = (sort.Contains("descending") ? sort.Substring(0, sort.IndexOf(" ")) : sort + "_descending");
                return membersList;
            }
            else
            {
                return membersList.OrderByDescending(x => x.Id).ToList();
            }
        }

        public ActionResult DoesUserExist(string SocialSecurityNumber) => Json(!db.Members.Any(x => x.SocialSecurityNumber.Equals(SocialSecurityNumber)), JsonRequestBehavior.AllowGet);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OverView(MemberSearchParams searchParams, string sort)
        {
            var list = db.Members.ToList();

            if (Request.Form["show"] != null)
            {
                ModelState.Clear();
            }
            else
            {
                list = SearchInList(searchParams, list);
            }

            list = SortList(ref sort, list);
            
            return View(new MembersOverViewViewModel { Members = list, SearchParams = searchParams, Sort = sort});
        }

        private static List<Member> SearchInList(MemberSearchParams searchParams, List<Member> list)
        {
            list = list
                .Where(x => (!string.IsNullOrWhiteSpace(searchParams.SocialNr)) ? x.SocialSecurityNumber.ToLower().Contains(searchParams.SocialNr.ToLower()) : true)
                .Where(x => (!string.IsNullOrWhiteSpace(searchParams.FirstName)) ? x.FirstName.ToLower().Contains(searchParams.FirstName.ToLower()) : true)
                .Where(x => (!string.IsNullOrWhiteSpace(searchParams.LastName)) ? x.LastName.ToLower().Contains(searchParams.LastName.ToLower()) : true)
                .Where(x => (searchParams.DateOfBirth != null) ? x.DateOfBirth.Date.Equals(searchParams.DateOfBirth.Value.Date) : true)
                .ToList();
            return list;
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
                return RedirectToAction("OverView", new { hasAddedMember = true, msg = $"{member.FullName} has been added successfully"});
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
