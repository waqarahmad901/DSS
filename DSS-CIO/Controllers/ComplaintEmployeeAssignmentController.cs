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
    public class ComplaintEmployeeAssignmentController : Controller
    {
        private DSS_CIO2Entities db = new DSS_CIO2Entities();

        // GET: /ComplaintEmployeeAssignment/
        public ActionResult Index()
        {
            var complaintemployeeassignments = db.ComplaintEmployeeAssignments.Include(c => c.Complaint).Include(c => c.Person);
            return View(complaintemployeeassignments.ToList());
        }

        // GET: /ComplaintEmployeeAssignment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComplaintEmployeeAssignment complaintemployeeassignment = db.ComplaintEmployeeAssignments.Find(id);
            if (complaintemployeeassignment == null)
            {
                return HttpNotFound();
            }
            return View(complaintemployeeassignment);
        }

        // GET: /ComplaintEmployeeAssignment/Create
        public ActionResult Create()
        {
            ViewBag.ComplaintID = new SelectList(db.Complaints, "ComplaintID", "Category");
            ViewBag.EmployeeID = new SelectList(db.People, "CNIC", "FName");
            return View();
        }

        // POST: /ComplaintEmployeeAssignment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ComplaintAssignmentID,ComplaintID,EmployeeID,Status,AssignmentDate,Deadline,ResolutionDate")] ComplaintEmployeeAssignment complaintemployeeassignment)
        {
            if (ModelState.IsValid)
            {
                db.ComplaintEmployeeAssignments.Add(complaintemployeeassignment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ComplaintID = new SelectList(db.Complaints, "ComplaintID", "Category", complaintemployeeassignment.ComplaintID);
            ViewBag.EmployeeID = new SelectList(db.People, "CNIC", "FName", complaintemployeeassignment.EmployeeID);
            return View(complaintemployeeassignment);
        }

        // GET: /ComplaintEmployeeAssignment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComplaintEmployeeAssignment complaintemployeeassignment = db.ComplaintEmployeeAssignments.Find(id);
            if (complaintemployeeassignment == null)
            {
                return HttpNotFound();
            }
            ViewBag.ComplaintID = new SelectList(db.Complaints, "ComplaintID", "Category", complaintemployeeassignment.ComplaintID);
            ViewBag.EmployeeID = new SelectList(db.People, "CNIC", "FName", complaintemployeeassignment.EmployeeID);
            return View(complaintemployeeassignment);
        }

        // POST: /ComplaintEmployeeAssignment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ComplaintAssignmentID,ComplaintID,EmployeeID,Status,AssignmentDate,Deadline,ResolutionDate")] ComplaintEmployeeAssignment complaintemployeeassignment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(complaintemployeeassignment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ComplaintID = new SelectList(db.Complaints, "ComplaintID", "Category", complaintemployeeassignment.ComplaintID);
            ViewBag.EmployeeID = new SelectList(db.People, "CNIC", "FName", complaintemployeeassignment.EmployeeID);
            return View(complaintemployeeassignment);
        }

        // GET: /ComplaintEmployeeAssignment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComplaintEmployeeAssignment complaintemployeeassignment = db.ComplaintEmployeeAssignments.Find(id);
            if (complaintemployeeassignment == null)
            {
                return HttpNotFound();
            }
            return View(complaintemployeeassignment);
        }

        // POST: /ComplaintEmployeeAssignment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ComplaintEmployeeAssignment complaintemployeeassignment = db.ComplaintEmployeeAssignments.Find(id);
            db.ComplaintEmployeeAssignments.Remove(complaintemployeeassignment);
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
