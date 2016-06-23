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
    public class JobDescriptionController : Controller
    {
        private DSS_CIO2Entities db = new DSS_CIO2Entities();

        // GET: /JobDescription/
        public ActionResult Index()
        {
         
            return View( );
        }

        // GET: /JobDescription/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobDescription jobdescription = db.JobDescriptions.Find(id);
            if (jobdescription == null)
            {
                return HttpNotFound();
            }
            return View(jobdescription);
        }

        // GET: /JobDescription/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName");
            return View();
        }

        // POST: /JobDescription/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="JobDescriptionID,JobTitle,DepartmentID,Salary")] JobDescription jobdescription)
        {
            if (ModelState.IsValid)
            {
                db.JobDescriptions.Add(jobdescription);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName", jobdescription.DepartmentID);
            return View(jobdescription);
        }

        // GET: /JobDescription/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobDescription jobdescription = db.JobDescriptions.Find(id);
            if (jobdescription == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName", jobdescription.DepartmentID);
            return View(jobdescription);
        }

        // POST: /JobDescription/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="JobDescriptionID,JobTitle,DepartmentID,Salary")] JobDescription jobdescription)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobdescription).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName", jobdescription.DepartmentID);
            return View(jobdescription);
        }

        // GET: /JobDescription/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobDescription jobdescription = db.JobDescriptions.Find(id);
            if (jobdescription == null)
            {
                return HttpNotFound();
            }
            return View(jobdescription);
        }

        // POST: /JobDescription/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobDescription jobdescription = db.JobDescriptions.Find(id);
            db.JobDescriptions.Remove(jobdescription);
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

        public ActionResult GetJobDescriptionList()
        {
            datatableModel model = new datatableModel();
            model.data = new List<string[]>();


            var list = db.JobDescriptions.ToList();
            foreach (var item in list)
            {
                model.data.Add(new string[] { item.JobTitle, item.Salary.ToString(),
                    "<a href='/JobDescription/Edit/" + item.JobDescriptionID + "'>Edit</a> | <a href='/JobDescription/Details/" + item.JobDescriptionID + "'>Details</a> | <a href='/JobDescription/Delete/" + item.JobDescriptionID + "'>Delete</a>" });
            }
            return Json(model, JsonRequestBehavior.AllowGet);

        }
    }
}
