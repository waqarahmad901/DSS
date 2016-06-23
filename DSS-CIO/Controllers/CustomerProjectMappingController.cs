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
    public class CustomerProjectMappingController : Controller
    {
        private DSS_CIO2Entities db = new DSS_CIO2Entities();

        // GET: /CustomerProjectMapping/
        public ActionResult Index()
        {
            var customerprojectmappings = db.CustomerProjectMappings.Include(c => c.Customer).Include(c => c.Project);
            return View(customerprojectmappings.ToList());
        }

        // GET: /CustomerProjectMapping/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerProjectMapping customerprojectmapping = db.CustomerProjectMappings.Where(x=>x.CustomerProjectId == id).FirstOrDefault();
            if (customerprojectmapping == null)
            {
                return HttpNotFound();
            }
            return View(customerprojectmapping);
        }

        // GET: /CustomerProjectMapping/Create
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerCNIC", "Contact");
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name");
            return View();
        }

        // POST: /CustomerProjectMapping/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="CustomerProjectID,CustomerID,ProjectID,SatisfactionLevel,TotalComplaints")] CustomerProjectMapping customerprojectmapping)
        {
            if (ModelState.IsValid)
            {
                db.CustomerProjectMappings.Add(customerprojectmapping);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerCNIC", "CustomerFullName", customerprojectmapping.CustomerID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name", customerprojectmapping.ProjectID);
            return View(customerprojectmapping);
        }

        // GET: /CustomerProjectMapping/Edit/5
        public ActionResult Edit(int? id, string customerid)
        {
            CustomerProjectMapping customerprojectmapping = null;
            if (id == 0)
            {
                customerprojectmapping = new CustomerProjectMapping();
            }
            else
            customerprojectmapping = db.CustomerProjectMappings.Where(x=>x.CustomerProjectId == id).FirstOrDefault();
            
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerCNIC", "CustomerFullName", customerid);
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name", customerprojectmapping.ProjectID);
            return View(customerprojectmapping);
        }

        // POST: /CustomerProjectMapping/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="CustomerProjectID,CustomerID,ProjectID,SatisfactionLevel,TotalComplaints")] CustomerProjectMapping customerprojectmapping)
        {
            if (ModelState.IsValid)
            {
                if(customerprojectmapping.CustomerProjectId == 0)
                db.Entry(customerprojectmapping).State = EntityState.Added;
                else
                db.Entry(customerprojectmapping).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details","Customer", new {id= customerprojectmapping.CustomerID.Trim() });
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerCNIC", "Contact", customerprojectmapping.CustomerID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name", customerprojectmapping.ProjectID);
            return View(customerprojectmapping);
        }

        // GET: /CustomerProjectMapping/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerProjectMapping customerprojectmapping = db.CustomerProjectMappings.Where(x=>x.CustomerProjectId == id).FirstOrDefault();
            if (customerprojectmapping == null)
            {
                return HttpNotFound();
            }
            return View(customerprojectmapping);
        }

        // POST: /CustomerProjectMapping/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerProjectMapping customerprojectmapping = db.CustomerProjectMappings.Where(x=>x.CustomerProjectId == id).FirstOrDefault();
            db.CustomerProjectMappings.Remove(customerprojectmapping);
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
