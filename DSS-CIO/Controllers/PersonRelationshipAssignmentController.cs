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
    public class PersonRelationshipAssignmentController : Controller
    {
        private DSS_CIO2Entities db = new DSS_CIO2Entities();

        // GET: /PersonRelationshipAssignment/
        public ActionResult Index()
        {
            var personrelationshipassignments = db.PersonRelationshipAssignments.Include(p => p.Person).Include(p => p.Person1);
            return View(personrelationshipassignments.ToList());
        }

        // GET: /PersonRelationshipAssignment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonRelationshipAssignment personrelationshipassignment = db.PersonRelationshipAssignments.Find(id);
            if (personrelationshipassignment == null)
            {
                return HttpNotFound();
            }
            return View(personrelationshipassignment);
        }

        // GET: /PersonRelationshipAssignment/Create
        public ActionResult Create()
        {
            ViewBag.Person1ID = new SelectList(db.People, "CNIC", "FName");
            ViewBag.Person2ID = new SelectList(db.People, "CNIC", "FName");
            return View();
        }

        // POST: /PersonRelationshipAssignment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RelationshipID,Person1ID,Person2ID,Relationship")] PersonRelationshipAssignment personrelationshipassignment, [Bind(Include = "RelationshipID,Person2ID,Person1ID,Relationship")] PersonRelationshipAssignment personrelationshipassignment1)
        {
            personrelationshipassignment1.Person1ID = personrelationshipassignment.Person2ID;
            personrelationshipassignment1.Person2ID = personrelationshipassignment.Person1ID;
            if (personrelationshipassignment.Relationship == "Father")
            {
                personrelationshipassignment1.Relationship = "Son";
            }
            else if (personrelationshipassignment1.Relationship == "Son")
            {
                personrelationshipassignment1.Relationship = "Father";
            }
            else
            {

            }

            if (ModelState.IsValid)
            {
                db.PersonRelationshipAssignments.Add(personrelationshipassignment1);
                db.PersonRelationshipAssignments.Add(personrelationshipassignment);
                db.SaveChanges();
                return RedirectToAction("Details", "Person", new { id = personrelationshipassignment.Person1ID });
            }

            ViewBag.Person1ID = new SelectList(db.People, "CNIC", "FName", personrelationshipassignment.Person1ID);
            ViewBag.Person2ID = new SelectList(db.People, "CNIC", "FName", personrelationshipassignment.Person2ID);
            return View(personrelationshipassignment);
        }

        // GET: /PersonRelationshipAssignment/Edit/5
        public ActionResult Edit(int? id,string cnic)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonRelationshipAssignment personrelationshipassignment = db.PersonRelationshipAssignments.Find(id);
            if (personrelationshipassignment == null)
            {
                personrelationshipassignment = new PersonRelationshipAssignment();
            }
            List<SelectListItem> item = new List<SelectListItem>();
            item.Add(new SelectListItem { Text = "Father", Value = "Father" });
            item.Add(new SelectListItem { Text = "Brother", Value = "Brother" });
            item.Add(new SelectListItem { Text = "Son", Value = "Son" });
            item.Add(new SelectListItem { Text = "Daughter", Value = "Daughter" });



            ViewBag.Person1ID = new SelectList(db.People, "CNIC", "FName", cnic);
            ViewBag.Person2ID = new SelectList(db.People, "CNIC", "FName", personrelationshipassignment.Person2ID);
            ViewBag.Relationship = new SelectList(item, "Value", "Text", personrelationshipassignment.Relationship);
            ViewBag.Relationship2 = new SelectList(item, "Value", "Text", personrelationshipassignment.Relationship2);
            return View(personrelationshipassignment);
        }

        // POST: /PersonRelationshipAssignment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RelationshipID,Person1ID,Person2ID,Relationship,Relationship2")] PersonRelationshipAssignment personrelationshipassignment)
        {
            if (ModelState.IsValid)
            {
                if(personrelationshipassignment.RelationshipID == 0)
                    db.Entry(personrelationshipassignment).State = EntityState.Added;
                else
                     db.Entry(personrelationshipassignment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Person", new { id = personrelationshipassignment.Person1ID });
            }
            ViewBag.Person1ID = new SelectList(db.People, "CNIC", "FName", personrelationshipassignment.Person1ID);
            ViewBag.Person2ID = new SelectList(db.People, "CNIC", "FName", personrelationshipassignment.Person2ID);
            return View(personrelationshipassignment);
        }

        // GET: /PersonRelationshipAssignment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonRelationshipAssignment personrelationshipassignment = db.PersonRelationshipAssignments.Find(id);
            if (personrelationshipassignment == null)
            {
                return HttpNotFound();
            }
            return View(personrelationshipassignment);
        }

        // POST: /PersonRelationshipAssignment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PersonRelationshipAssignment personrelationshipassignment = db.PersonRelationshipAssignments.Find(id);
            string PersonID = personrelationshipassignment.Person1ID;
            db.PersonRelationshipAssignments.Remove(personrelationshipassignment);
            db.SaveChanges();
            return RedirectToAction("Details", "Person", new { id = PersonID });
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
