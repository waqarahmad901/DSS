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
    public class MilestoneController : Controller
    {
        private DSS_CIO2Entities db = new DSS_CIO2Entities();

        // GET: /Milestone/
        public ActionResult Index()
        {
            var milestones = db.Milestones.Include(m => m.Project);
            return View(milestones.ToList());
        }

        // GET: /Milestone/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Milestone milestone = db.Milestones.Find(id);
            if (milestone == null)
            {
                return HttpNotFound();
            }
            return View(milestone);
        }

        // GET: /Milestone/Create
        public ActionResult Create()
        {
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name");
            return View();
        }

        // POST: /Milestone/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MilestoneID,Description,TotalDeliverables,ProjectID,AssignmentDate,Deadline,ActualSubmissionDate")] Milestone milestone)
        {
            if (ModelState.IsValid)
            {
                db.Milestones.Add(milestone);
                db.SaveChanges();
                return RedirectToAction("Details", "Project", new { id = milestone.ProjectID });
            }

            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name", milestone.ProjectID);
            return View(milestone);
        }

        // GET: /Milestone/Edit/5
        public ActionResult Edit(int? id,int projectid)
        {
            Milestone milestone = null;
            if (id == 0)
            {
                milestone = new Milestone();
            }
            else
                milestone = db.Milestones.Find(id);

            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name", projectid);
            return View(milestone);
        }

        // POST: /Milestone/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MilestoneID,Description,TotalDeliverables,ProjectID,AssignmentDate,Deadline,ActualSubmissionDate")] Milestone milestone)
        {
            if (ModelState.IsValid)
            {
                if(milestone.MilestoneID == 0)
                db.Entry(milestone).State = EntityState.Added;
                else
                db.Entry(milestone).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Project", new { id = milestone.ProjectID });
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name", milestone.ProjectID);
            return View(milestone);
        }

        // GET: /Milestone/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Milestone milestone = db.Milestones.Find(id);
            if (milestone == null)
            {
                return HttpNotFound();
            }
            return View(milestone);
        }

        // POST: /Milestone/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Milestone milestone = db.Milestones.Find(id);
            int? identification = milestone.ProjectID; 
            db.Milestones.Remove(milestone);
            db.SaveChanges();
            return RedirectToAction("Details", "Project", new { id = identification });
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
