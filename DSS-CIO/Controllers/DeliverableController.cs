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
    public class DeliverableController : Controller
    {
        private DSS_CIO2Entities db = new DSS_CIO2Entities();

        // GET: /Deliverable/
        public ActionResult Index()
        {
            var deliverables = db.Deliverables.Include(d => d.Milestone).Include(d => d.Project);
            return View(deliverables.ToList());
        }

        // GET: /Deliverable/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deliverable deliverable = db.Deliverables.Find(id);
            if (deliverable == null)
            {
                return HttpNotFound();
            }
            return View(deliverable);
        }

        // GET: /Deliverable/Create
        public ActionResult Create()
        {
            ViewBag.MilestoneID = new SelectList(db.Milestones, "MilestoneID", "Description");
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name");
            return View();
        }

        // POST: /Deliverable/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="TaskID,Description,AssignedBy,Version,ReviewedBy,MilestoneID,ProjectID")] Deliverable deliverable)
        {
            if (ModelState.IsValid)
            {
                db.Deliverables.Add(deliverable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MilestoneID = new SelectList(db.Milestones, "MilestoneID", "Description", deliverable.MilestoneID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name", deliverable.ProjectID);
            return View(deliverable);
        }

        // GET: /Deliverable/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deliverable deliverable = db.Deliverables.Find(id);
            if (deliverable == null)
            {
                return HttpNotFound();
            }
            ViewBag.MilestoneID = new SelectList(db.Milestones, "MilestoneID", "Description", deliverable.MilestoneID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name", deliverable.ProjectID);
            return View(deliverable);
        }

        // POST: /Deliverable/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="TaskID,Description,AssignedBy,Version,ReviewedBy,MilestoneID,ProjectID")] Deliverable deliverable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deliverable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MilestoneID = new SelectList(db.Milestones, "MilestoneID", "Description", deliverable.MilestoneID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name", deliverable.ProjectID);
            return View(deliverable);
        }

        // GET: /Deliverable/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deliverable deliverable = db.Deliverables.Find(id);
            if (deliverable == null)
            {
                return HttpNotFound();
            }
            return View(deliverable);
        }

        // POST: /Deliverable/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Deliverable deliverable = db.Deliverables.Find(id);
            db.Deliverables.Remove(deliverable);
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
