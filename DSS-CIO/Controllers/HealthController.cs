using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DSS_CIO.Models;

namespace DSS_CIO.Controllers
{
    [AuthorizeUser(AccessLevel = "data entery operator")]
    public class HealthController : Controller
    {
        private DSS_CIO2Entities db = new DSS_CIO2Entities();

        // GET: /Health/
        public ActionResult Index()
        {
            var healths = db.Healths.Include(h => h.Person);
            return View(healths.ToList());
        }

        // GET: /Health/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Health health = db.Healths.Find(id);
            if (health == null)
            {
                return HttpNotFound();
            }
            return View(health);
        }

        // GET: /Health/Create
        public ActionResult Create()
        {
            ViewBag.PersonID = new SelectList(db.People, "CNIC", "FName");
            return View();
        }

        // POST: /Health/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="HealthID,PersonID,BloodGroup,BMI,EyeSight,Allergy,Disease,OtherHealthIssue")] Health health)
        {
            if (ModelState.IsValid)
            {
                db.Healths.Add(health);
                db.SaveChanges();
                return RedirectToAction("Details", "Person", new { id = health.PersonID });
            }

            ViewBag.PersonID = new SelectList(db.People, "CNIC", "FName", health.PersonID);
            return View(health);
        }

        // GET: /Health/Edit/5
        public ActionResult Edit(int? id,string cnic)
        {
            Health health = null;
            if (id == 0)
            {
                health = new Health();
            }
            else
             health = db.Healths.Find(id);
           
            ViewBag.PersonID = new SelectList(db.People, "CNIC", "FName", cnic);
            return View(health);
        }

        // POST: /Health/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="HealthID,PersonID,BloodGroup,BMI,EyeSight,Allergy,Disease,OtherHealthIssue")] Health health)
        {
            if (ModelState.IsValid)
            {
                if(health.HealthID == 0)
                db.Entry(health).State = EntityState.Added;
                else
                db.Entry(health).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Person", new { id = health.PersonID });
            }
            ViewBag.PersonID = new SelectList(db.People, "CNIC", "FName", health.PersonID);
            return View(health);
        }

        // GET: /Health/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Health health = db.Healths.Find(id);
            if (health == null)
            {
                return HttpNotFound();
            }
            return View(health);
        }

        // POST: /Health/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Health health = db.Healths.Find(id);
            db.Healths.Remove(health);
            db.SaveChanges();
            return RedirectToAction("Details", "Person", new { id = health.PersonID });
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
