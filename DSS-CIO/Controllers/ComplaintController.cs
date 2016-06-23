using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DSS_CIO.Models;
using DSSCIO.Areas.Test.Models;

namespace DSS_CIO.Controllers
{
    [AuthorizeUser(AccessLevel = "data entery operator")]
    public class ComplaintController : Controller
    {
        private DSS_CIO2Entities db = new DSS_CIO2Entities();

        // GET: /Complaint/
        public ActionResult Index()
        {
             return View( );
        }

        // GET: /Complaint/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Complaint complaint = db.Complaints.Find(id);
            if (complaint == null)
            {
                return HttpNotFound();
            }
            return View(complaint);
        }

        // GET: /Complaint/Create
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerCNIC", "Contact");
            ViewBag.MilestoneID = new SelectList(db.Milestones, "MilestoneID", "Description");
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name");
            ViewBag.TaskID = new SelectList(db.Deliverables, "TaskID", "Description");
            return View();
        }

        // POST: /Complaint/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ComplaintID,Category,Description,ProjectID,MilestoneID,TaskID,CustomerID,Severity")] Complaint complaint)
        {
            if (ModelState.IsValid)
            {
                db.Complaints.Add(complaint);
                db.SaveChanges();
                return RedirectToAction("Details", "Deliverable", new { id = complaint.TaskID });
            }

            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerCNIC", "Contact", complaint.CustomerID);
            ViewBag.MilestoneID = new SelectList(db.Milestones, "MilestoneID", "Description", complaint.MilestoneID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name", complaint.ProjectID);
            ViewBag.TaskID = new SelectList(db.Deliverables, "TaskID", "Description", complaint.TaskID);
            return View(complaint);
        }

        // GET: /Complaint/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Complaint complaint = db.Complaints.Find(id);
            if (complaint == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerCNIC", "Contact", complaint.CustomerID);
            ViewBag.MilestoneID = new SelectList(db.Milestones, "MilestoneID", "Description", complaint.MilestoneID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name", complaint.ProjectID);
            ViewBag.TaskID = new SelectList(db.Deliverables, "TaskID", "Description", complaint.TaskID);
            return View(complaint);
        }

        // POST: /Complaint/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ComplaintID,Category,Description,ProjectID,MilestoneID,TaskID,CustomerID,Severity")] Complaint complaint)
        {
            if (ModelState.IsValid)
            {
                db.Entry(complaint).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Deliverable", new { id = complaint.TaskID });
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerCNIC", "Contact", complaint.CustomerID);
            ViewBag.MilestoneID = new SelectList(db.Milestones, "MilestoneID", "Description", complaint.MilestoneID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name", complaint.ProjectID);
            ViewBag.TaskID = new SelectList(db.Deliverables, "TaskID", "Description", complaint.TaskID);
            return View(complaint);
        }

        // GET: /Complaint/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Complaint complaint = db.Complaints.Find(id);
            if (complaint == null)
            {
                return HttpNotFound();
            }
            return View(complaint);
        }

        // POST: /Complaint/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Complaint complaint = db.Complaints.Find(id);
            int? identification = complaint.TaskID;
            db.Complaints.Remove(complaint);
            db.SaveChanges();
            return RedirectToAction("Details", "Deliverable", new { id = identification });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult GetComplaintList()
        {
            datatableModel model = new datatableModel();
            model.data = new List<string[]>();


            var list = db.Complaints.ToList();
            foreach (var item in list)
            {
                model.data.Add(new string[] { item.Category, item.Description, item.Severity.ToString(), item.Customer.Contact,item.Milestone.Description, item.Project.Name,item.Deliverable.Description,
                    "<a href='/Complaint/Edit/" + item.ComplaintID + "'>Edit</a> | <a href='/Complaint/Details/" + item.ComplaintID + "'>Details</a> | <a href='/Complaint/Delete/" + item.ComplaintID + "'>Delete</a>" });
            }
            return Json(model, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult GetMileStoneByProjectId(int id)
        {
            var model = db.Milestones.Where(x => x.ProjectID == id).Select(x => new { key = x.MilestoneID, value = x.Description }).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTaskBymileStoneId(int id)
        {
            var model = db.Deliverables.Where(x => x.MilestoneID== id).Select(x => new { key = x.TaskID, value = x.Description}).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}
