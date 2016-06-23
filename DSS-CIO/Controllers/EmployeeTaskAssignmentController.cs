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
    public class EmployeeTaskAssignmentController : Controller
    {
        private DSS_CIO2Entities db = new DSS_CIO2Entities();

        // GET: /EmployeeTaskAssignment/
        public ActionResult Index()
        {
            var employeetaskassignments = db.EmployeeTaskAssignments.Include(e => e.Deliverable).Include(e => e.Person).Include(e => e.Project);
            return View(employeetaskassignments.ToList());
        }

        // GET: /EmployeeTaskAssignment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeTaskAssignment employeetaskassignment = db.EmployeeTaskAssignments.Find(id);
            if (employeetaskassignment == null)
            {
                return HttpNotFound();
            }
            return View(employeetaskassignment);
        }

        // GET: /EmployeeTaskAssignment/Create
        public ActionResult Create()
        {
            ViewBag.TaskID = new SelectList(db.Deliverables, "TaskID", "Description");
            ViewBag.EmployeeID = new SelectList(db.People, "CNIC", "FName");
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name");
            return View();
        }

        // POST: /EmployeeTaskAssignment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,TaskID,EmployeeID,AssignmentDate,Deadline,SubmissionDate,ProjectID")] EmployeeTaskAssignment employeetaskassignment)
        {
            if (ModelState.IsValid)
            {
                db.EmployeeTaskAssignments.Add(employeetaskassignment);
                db.SaveChanges();
                return RedirectToAction("Details", "Project", new { id = employeetaskassignment.ProjectID });
            }

            ViewBag.TaskID = new SelectList(db.Deliverables, "TaskID", "Description", employeetaskassignment.TaskID);
            ViewBag.EmployeeID = new SelectList(db.People, "CNIC", "FName", employeetaskassignment.EmployeeID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name", employeetaskassignment.ProjectID);
            return View(employeetaskassignment);
        }

        // GET: /EmployeeTaskAssignment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeTaskAssignment employeetaskassignment = db.EmployeeTaskAssignments.Find(id);
            if (employeetaskassignment == null)
            {
                return HttpNotFound();
            }
            ViewBag.TaskID = new SelectList(db.Deliverables, "TaskID", "Description", employeetaskassignment.TaskID);
            ViewBag.EmployeeID = new SelectList(db.People, "CNIC", "FName", employeetaskassignment.EmployeeID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name", employeetaskassignment.ProjectID);
            return View(employeetaskassignment);
        }

        // POST: /EmployeeTaskAssignment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,TaskID,EmployeeID,AssignmentDate,Deadline,SubmissionDate,ProjectID")] EmployeeTaskAssignment employeetaskassignment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeetaskassignment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Project", new { id = employeetaskassignment.ProjectID });
            }
            ViewBag.TaskID = new SelectList(db.Deliverables, "TaskID", "Description", employeetaskassignment.TaskID);
            ViewBag.EmployeeID = new SelectList(db.People, "CNIC", "FName", employeetaskassignment.EmployeeID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name", employeetaskassignment.ProjectID);
            return View(employeetaskassignment);
        }

        // GET: /EmployeeTaskAssignment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeTaskAssignment employeetaskassignment = db.EmployeeTaskAssignments.Find(id);
            if (employeetaskassignment == null)
            {
                return HttpNotFound();
            }
            return View(employeetaskassignment);
        }

        // POST: /EmployeeTaskAssignment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmployeeTaskAssignment employeetaskassignment = db.EmployeeTaskAssignments.Find(id);
            db.EmployeeTaskAssignments.Remove(employeetaskassignment);
            db.SaveChanges();
            return RedirectToAction("Details", "Project", new { id = employeetaskassignment.ProjectID });
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
