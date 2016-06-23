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
    public class InanimateResourceController : Controller
    {
        private DSS_CIO2Entities db = new DSS_CIO2Entities();

        // GET: /InanimateResource/
        public ActionResult Index()
        { 
            return View( );
        }

        // GET: /InanimateResource/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InanimateResource inanimateresource = db.InanimateResources.Find(id);
            if (inanimateresource == null)
            {
                return HttpNotFound();
            }
            return View(inanimateresource);
        }

        // GET: /InanimateResource/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /InanimateResource/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ResourceID,Name,Model,Make,Price,Condition,ExpenditureID")] InanimateResource inanimateresource)
        {
            if (ModelState.IsValid)
            {
                db.InanimateResources.Add(inanimateresource);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(inanimateresource);
        }

        // GET: /InanimateResource/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InanimateResource inanimateresource = db.InanimateResources.Find(id);
            if (inanimateresource == null)
            {
                return HttpNotFound();
            }
            return View(inanimateresource);
        }

        // POST: /InanimateResource/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ResourceID,Name,Model,Make,Price,Condition,ExpenditureID")] InanimateResource inanimateresource)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inanimateresource).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(inanimateresource);
        }

        // GET: /InanimateResource/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InanimateResource inanimateresource = db.InanimateResources.Find(id);
            if (inanimateresource == null)
            {
                return HttpNotFound();
            }
            return View(inanimateresource);
        }

        // POST: /InanimateResource/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InanimateResource inanimateresource = db.InanimateResources.Find(id);
            db.InanimateResources.Remove(inanimateresource);
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
        public ActionResult GetResourceList()
        {
            datatableModel model = new datatableModel();
            model.data = new List<string[]>();


            var list = db.InanimateResources.ToList();
            foreach (var item in list)
            {
                model.data.Add(new string[] { item.Name, item.Model, item.Make, item.Price.ToString(),item.Condition,
                    "<a href='/InanimateResource/Edit/" + item.ResourceID + "'>Edit</a> | <a href='/InanimateResource/Details/" + item.ResourceID + "'>Details</a> | <a href='/InanimateResource/Delete/" + item.ResourceID + "'>Delete</a>" });
            }
            return Json(model, JsonRequestBehavior.AllowGet);

        }
    }
}
