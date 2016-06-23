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
    public class PersonController : Controller
    {
        private DSS_CIO2Entities db = new DSS_CIO2Entities();

        // GET: /Person/
        public ActionResult Index()
        { 
            return View( );
        }

        // GET: /Person/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: /Person/Create
        public ActionResult Create()
        {
            List<SelectListItem> item = new List<SelectListItem>();
            item.Add(new SelectListItem { Text = "Employee", Value = "Employee" });
            item.Add(new SelectListItem { Text = "Candidate", Value = "Candidate" });
            item.Add(new SelectListItem { Text = "Relative", Value = "Relative" });
            ViewBag.EmploymentStatus = new SelectList(item,"Value","Text");
            ViewBag.JobDescriptionID = new SelectList(db.JobDescriptions, "JobDescriptionID", "JobTitle");
            return View();
        }

        // POST: /Person/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include= "CNIC,FName,LName,DateOfBirth,Gender,EmploymentStatus,JobDescriptionID,Contact,AddressLine,Street,City,Country,PostalCode")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.People.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.JobDescriptionID = new SelectList(db.JobDescriptions, "JobDescriptionID", "JobTitle", person.JobDescriptionID);
            return View(person);
        }

        // GET: /Person/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            List<SelectListItem> item = new List<SelectListItem>();
            item.Add(new SelectListItem { Text = "Employee", Value = "Employee" });
            item.Add(new SelectListItem { Text = "Candidate", Value = "Candidate" });
            item.Add(new SelectListItem { Text = "Relative", Value = "Relative" });
            ViewBag.EmploymentStatus = new SelectList(item, "Value", "Text",person.EmploymentStatus);
            ViewBag.JobDescriptionID = new SelectList(db.JobDescriptions, "JobDescriptionID", "JobTitle", person.JobDescriptionID);
            return View(person);
        }

        // POST: /Person/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include= "CNIC,FName,LName,DateOfBirth,Gender,EmploymentStatus,JobDescriptionID,Contact,AddressLine,Street,City,Country,PostalCode")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.JobDescriptionID = new SelectList(db.JobDescriptions, "JobDescriptionID", "JobTitle", person.JobDescriptionID);
            return View(person);
        }

        // GET: /Person/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: /Person/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Person person = db.People.Find(id);
            db.People.Remove(person);
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

        public ActionResult GetPersonList()
        {
            datatableModel model = new datatableModel();
            model.data = new List<string[]>();

        
            var list = db.People.ToList();
            foreach (var item in list)
            {
                model.data.Add(new string[] { item.FName, item.LName, item.DateOfBirth.Value.ToShortDateString(), item.Gender
                    , item.Contact, item.JobDescription!= null ?  item.JobDescription.JobTitle : "", item.AddressLine, item.Street,item.City,
                    item.Country, item.PostalCode, "<a href='/Person/Edit/" + item.CNIC + "'>Edit</a> | <a href='/Person/Details/" + item.CNIC + "'>Details</a> | <a href='/Person/Delete/" + item.CNIC + "'>Delete</a>" });
            }
            return Json(model, JsonRequestBehavior.AllowGet);

        }
    }
}
