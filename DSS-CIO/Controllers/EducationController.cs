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
    public class EducationController : Controller
    {
        private DSS_CIO2Entities db = new DSS_CIO2Entities();

        // GET: /Education/
        public ActionResult Index()
        {
            var educations = db.Educations.Include(e => e.Person);
            return View(educations.ToList());
        }

        // GET: /Education/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Education education = db.Educations.Find(id);
            if (education == null)
            {
                return HttpNotFound();
            }
            return View(education);
        }

        // GET: /Education/Create
        public ActionResult Create()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Complete", Value = "Complete" });
            items.Add(new SelectListItem { Text = "In Progress", Value = "In Progress" });
            ViewBag.CompletionStatus = new SelectList(items, "Value", "Text", "");
            ViewBag.PersonID = new SelectList(db.People, "CNIC", "FName");
            return View();
        }

        // POST: /Education/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="EducationID,PersonID,DegreeName,DegreeType,Board_University,CGPA_Percentage,Awards,CompletionStatus,StartDate,EndDate")] Education education)
        {
            if (ModelState.IsValid)
            {
                db.Educations.Add(education);
                db.SaveChanges();
                return RedirectToAction("Details", "Person", new { id = education.PersonID });
            }
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Complete",Value = "Complete" });
            items.Add(new SelectListItem { Text = "In Progress", Value = "In Progress" });
            ViewBag.PersonID = new SelectList(db.People, "CNIC", "FName", education.PersonID);
            ViewBag.CompletionStatus = new SelectList(items, "Value", "Text", education.CompletionStatus);
            return View(education);
        }

        // GET: /Education/Edit/5
        public ActionResult Edit(int? id,string cnic)
        {
            Education education = null;
            if (id == 0)
                education = new Education();
            else 
                education = db.Educations.Find(id);

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Complete", Value = "Complete" });
            items.Add(new SelectListItem { Text = "In Progress", Value = "In Progress" });
            ViewBag.CompletionStatus = new SelectList(items, "Value", "Text", education.CompletionStatus);

            ViewBag.PersonID = new SelectList(db.People, "CNIC", "FName", cnic);
            return View(education);
        }

        // POST: /Education/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include= "EducationID,PersonID,DegreeName,DegreeType,Board_University,CGPA_Percentage,Awards,CompletionStatus,StartDate,EndDate")] Education education)
        {
            if (ModelState.IsValid)
            {
                if(education.EducationID == 0)
                    db.Entry(education).State = EntityState.Added;
                else
                    db.Entry(education).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Person", new { id = education.PersonID });
            }
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Complete", Value = "Complete" });
            items.Add(new SelectListItem { Text = "In Progress", Value = "In Progress" });
            ViewBag.PersonID = new SelectList(db.People, "CNIC", "FName", education.PersonID);
            ViewBag.CompletionStatus = new SelectList(items, "Value", "Text", education.CompletionStatus);
            return View(education);
        }

        // GET: /Education/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Education education = db.Educations.Find(id);
            if (education == null)
            {
                return HttpNotFound();
            }
            return View(education);
        }

        // POST: /Education/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Education education = db.Educations.Find(id);
            db.Educations.Remove(education);
            db.SaveChanges();
            return RedirectToAction("Details", "Person", new { id = education.PersonID });
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
