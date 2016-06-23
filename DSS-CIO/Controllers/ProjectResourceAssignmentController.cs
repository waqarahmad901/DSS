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
    public class ProjectResourceAssignmentController : Controller
    {
        private DSS_CIO2Entities db = new DSS_CIO2Entities();

        // GET: /ProjectResourceAssignment/
        public ActionResult Index()
        {
            var projectresourceassignments = db.ProjectResourceAssignments.Include(p => p.InanimateResource).Include(p => p.Project).Include(p => p.Person);
            return View(projectresourceassignments.ToList());
        }

        // GET: /ProjectResourceAssignment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectResourceAssignment projectresourceassignment = db.ProjectResourceAssignments.Find(id);
            if (projectresourceassignment == null)
            {
                return HttpNotFound();
            }
            return View(projectresourceassignment);
        }

        // GET: /ProjectResourceAssignment/Create
        public ActionResult Create()
        {
            ViewBag.ResourceID = new SelectList(db.InanimateResources, "ResourceID", "Name");
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name");
            ViewBag.EmployeeID = new SelectList(db.People, "CNIC", "FName");
            return View();
        }

        // POST: /ProjectResourceAssignment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ProjectResourceID,ProjectID,ResourceID,Status,AssignmentDate,ExpectedReturnDate,ActualReturnDate,EmployeeID")] ProjectResourceAssignment projectresourceassignment)
        {
            if (ModelState.IsValid)
            {
                db.ProjectResourceAssignments.Add(projectresourceassignment);
                db.SaveChanges();
                return RedirectToAction("Details", "Project", new { id = projectresourceassignment.ProjectID });
            }

            ViewBag.ResourceID = new SelectList(db.InanimateResources, "ResourceID", "Name", projectresourceassignment.ResourceID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name", projectresourceassignment.ProjectID);
            ViewBag.EmployeeID = new SelectList(db.People, "CNIC", "FName", projectresourceassignment.EmployeeID);
            return View(projectresourceassignment);
        }

        // GET: /ProjectResourceAssignment/Edit/5
        public ActionResult Edit(int? id,int projectid)
        {
            ProjectResourceAssignment projectresourceassignment = null;
            if (id == 0)
            {
                projectresourceassignment = new ProjectResourceAssignment();
            }
            else
             projectresourceassignment = db.ProjectResourceAssignments.Find(id);
      
            ViewBag.ResourceID = new SelectList(db.InanimateResources, "ResourceID", "Name", projectresourceassignment.ResourceID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name", projectid);
            ViewBag.EmployeeID = new SelectList(db.People, "CNIC", "FName", projectresourceassignment.EmployeeID);
            return View(projectresourceassignment);
        }

        // POST: /ProjectResourceAssignment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ProjectResourceID,ProjectID,ResourceID,Status,AssignmentDate,ExpectedReturnDate,ActualReturnDate,EmployeeID")] ProjectResourceAssignment projectresourceassignment)
        {
            if (ModelState.IsValid)
            {
                if(projectresourceassignment.ProjectResourceID == 0)
                db.Entry(projectresourceassignment).State = EntityState.Added;
                else
                db.Entry(projectresourceassignment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Project", new { id = projectresourceassignment.ProjectID });
            }
            ViewBag.ResourceID = new SelectList(db.InanimateResources, "ResourceID", "Name", projectresourceassignment.ResourceID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name", projectresourceassignment.ProjectID);
            ViewBag.EmployeeID = new SelectList(db.People, "CNIC", "FName", projectresourceassignment.EmployeeID);
            return View(projectresourceassignment);
        }

        // GET: /ProjectResourceAssignment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectResourceAssignment projectresourceassignment = db.ProjectResourceAssignments.Find(id);
            if (projectresourceassignment == null)
            {
                return HttpNotFound();
            }
            return View(projectresourceassignment);
        }

        // POST: /ProjectResourceAssignment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectResourceAssignment projectresourceassignment = db.ProjectResourceAssignments.Find(id);
            db.ProjectResourceAssignments.Remove(projectresourceassignment);
            db.SaveChanges();
            return RedirectToAction("Details", "Project", new { id = projectresourceassignment.ProjectID });
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
