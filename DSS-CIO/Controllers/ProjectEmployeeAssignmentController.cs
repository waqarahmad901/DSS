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
    public class ProjectEmployeeAssignmentController : Controller
    {
        private DSS_CIO2Entities db = new DSS_CIO2Entities();

        // GET: /ProjectEmployeeAssignment/
        public ActionResult Index()
        {
            var projectemployeeassignments = db.ProjectEmployeeAssignments.Include(p => p.Person).Include(p => p.Project);
            return View(projectemployeeassignments.ToList());
        }

        // GET: /ProjectEmployeeAssignment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectEmployeeAssignment projectemployeeassignment = db.ProjectEmployeeAssignments.Find(id);
            if (projectemployeeassignment == null)
            {
                return HttpNotFound();
            }
            return View(projectemployeeassignment);
        }

        // GET: /ProjectEmployeeAssignment/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeID = new SelectList(db.People, "CNIC", "FName");
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name");
            return View();
        }

        // POST: /ProjectEmployeeAssignment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ProjectEmployeeID,ProjectID,EmployeeID,Role,AppointmentDate,Performance")] ProjectEmployeeAssignment projectemployeeassignment)
        {
            if (ModelState.IsValid)
            {
                db.ProjectEmployeeAssignments.Add(projectemployeeassignment);
                db.SaveChanges();
                return RedirectToAction("Details", "Project", new { id = projectemployeeassignment.ProjectID });
            }

            ViewBag.EmployeeID = new SelectList(db.People, "CNIC", "FName", projectemployeeassignment.EmployeeID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name", projectemployeeassignment.ProjectID);
            return View(projectemployeeassignment);
        }

        // GET: /ProjectEmployeeAssignment/Edit/5
        public ActionResult Edit(int? id,int projectid)
        {
            ProjectEmployeeAssignment projectemployeeassignment = null;
            if (id == 0)
            {
                projectemployeeassignment = new ProjectEmployeeAssignment();
            }
            else
             projectemployeeassignment = db.ProjectEmployeeAssignments.Find(id);
        
            ViewBag.EmployeeID = new SelectList(db.People, "CNIC", "FName", projectemployeeassignment.EmployeeID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name", projectid);
            return View(projectemployeeassignment);
        }

        // POST: /ProjectEmployeeAssignment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ProjectEmployeeID,ProjectID,EmployeeID,Role,AppointmentDate,Performance")] ProjectEmployeeAssignment projectemployeeassignment)
        {
            if (ModelState.IsValid)
            {
                if(projectemployeeassignment.ProjectEmployeeID == 0)
                db.Entry(projectemployeeassignment).State = EntityState.Added;
                else
                db.Entry(projectemployeeassignment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Project", new { id = projectemployeeassignment.ProjectID });
            }
            ViewBag.EmployeeID = new SelectList(db.People, "CNIC", "FName", projectemployeeassignment.EmployeeID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name", projectemployeeassignment.ProjectID);
            return View(projectemployeeassignment);
        }

        // GET: /ProjectEmployeeAssignment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectEmployeeAssignment projectemployeeassignment = db.ProjectEmployeeAssignments.Find(id);
            if (projectemployeeassignment == null)
            {
                return HttpNotFound();
            }
            return View(projectemployeeassignment);
        }

        // POST: /ProjectEmployeeAssignment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectEmployeeAssignment projectemployeeassignment = db.ProjectEmployeeAssignments.Find(id);
            db.ProjectEmployeeAssignments.Remove(projectemployeeassignment);
            db.SaveChanges();
            return RedirectToAction("Details", "Project", new { id = projectemployeeassignment.ProjectID });
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
