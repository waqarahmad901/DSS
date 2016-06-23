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
    public class CertificationController : Controller
    {
        private DSS_CIO2Entities db = new DSS_CIO2Entities();
       

        // GET: /Certification/
        public ActionResult Index()
        {
            var certifications = db.Certifications.Include(c => c.Person);
            return View(certifications.ToList());
        }

        // GET: /Certification/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Certification certification = db.Certifications.Find(id);
            if (certification == null)
            {
                return HttpNotFound();
            }
            return View(certification);
        }

        // GET: /Certification/Create
        public ActionResult Create()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Self", Value = "Self" });
            items.Add(new SelectListItem { Text = "Accepted", Value = "Accepted" });
            items.Add(new SelectListItem { Text = "Rejected", Value = "Rejected" });

            ViewBag.ApplicationStatus = new SelectList(items, "Value", "Text", "");
            List<SelectListItem> items2 = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Complete", Value = "Complete" });
            items.Add(new SelectListItem { Text = "In Progress", Value = "In Progress" });
            ViewBag.CompletionStatus = new SelectList(items2, "Value", "Text", "");
            

            ViewBag.PersonID = new SelectList(db.People, "CNIC", "FName");
            return View();
        }

        // POST: /Certification/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="CertificationID,PersonID,Name,Duration,CompletionStatus,Grade,ApplicationStatus,StartDate,EndDate")] Certification certification)
        {
            if (ModelState.IsValid)
            {
                db.Certifications.Add(certification);
                db.SaveChanges();
                return RedirectToAction("Details", "Person", new { id = certification.PersonID });
            }

            ViewBag.PersonID = new SelectList(db.People, "CNIC", "FName", certification.PersonID);
            return View(certification);
        }

        // GET: /Certification/Edit/5
        public ActionResult Edit(int? id,string cnic)
        {
            Certification certification = null;
            if (id == 0)
                certification = new Certification();
            else
                certification = db.Certifications.Find(id);
            
            if (certification == null)
            {
                return HttpNotFound();
            }
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Self", Value = "Self" });
            items.Add(new SelectListItem { Text = "Accepted", Value = "Accepted" });
            items.Add(new SelectListItem { Text = "Rejected", Value = "Rejected" });
            ViewBag.ApplicationStatus = new SelectList(items, "Value", "Text", "");
            List<SelectListItem> items2 = new List<SelectListItem>();
            items2.Add(new SelectListItem { Text = "Complete", Value = "Complete" });
            items2.Add(new SelectListItem { Text = "In Progress", Value = "In Progress" });
            ViewBag.CompletionStatus = new SelectList(items2, "Value", "Text", "");

            ViewBag.PersonID = new SelectList(db.People, "CNIC", "FName", cnic);
            return View(certification);
        }

        // POST: /Certification/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include= "CertificationID,PersonID,Name,Duration,CompletionStatus,Grade,ApplicationStatus,StartDate,EndDate")] Certification certification)
        {
            if (ModelState.IsValid)
            {
                if(certification.CertificationID == 0)
                    db.Entry(certification).State = EntityState.Added;
                else
                db.Entry(certification).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Person", new { id = certification.PersonID });
            }
            ViewBag.PersonID = new SelectList(db.People, "CNIC", "FName", certification.PersonID);
            return View(certification);
        }

        // GET: /Certification/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Certification certification = db.Certifications.Find(id);
            if (certification == null)
            {
                return HttpNotFound();
            }
            return View(certification);
        }

        // POST: /Certification/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Certification certification = db.Certifications.Find(id);
            db.Certifications.Remove(certification);
            db.SaveChanges();
            return RedirectToAction("Details", "Person", new { id = certification.PersonID });
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
