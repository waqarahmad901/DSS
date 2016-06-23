using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataAccess;
using DSS_CIO.Models;
using DSSCIO.Areas.Test.Models;

namespace DSS_CIO.Controllers
{
    [AuthorizeUser(AccessLevel = "data entery operator")]
    public class FinanceTransactionsController : Controller
    {
        private DSS_CIO2Entities db = new DSS_CIO2Entities();

        // GET: FinanceTransactions
        public ActionResult Index()
        {
           // var financeTransactions = db.FinanceTransactions.Include(f => f.Customer).Include(f => f.ProjectFinance).Include(f => f.Project);
            return View();
        }

        // GET: FinanceTransactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FinanceTransaction financeTransaction = db.FinanceTransactions.Find(id);
            if (financeTransaction == null)
            {
                return HttpNotFound();
            }
            return View(financeTransaction);
        }

        // GET: FinanceTransactions/Create
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerCNIC", "CustomerFullName");
            ViewBag.ProjectFinanceID = new SelectList(db.ProjectFinances, "ProjectFinanceID", "ProjectFinanceID");
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name");
            return View();
        }

        // POST: FinanceTransactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FinanceTransactionID,TransactionType,IdentificationDocumentNumber,IdentificationDocumentType,Amount,SenderIdentification,ReceiverIdentification,TransactionDate,ProjectFinanceID,ProjectID,CustomerID")] FinanceTransaction financeTransaction)
        {
            if (ModelState.IsValid)
            {
                db.FinanceTransactions.Add(financeTransaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerCNIC", "CustomerFullName", financeTransaction.CustomerID);
            ViewBag.ProjectFinanceID = new SelectList(db.ProjectFinances, "ProjectFinanceID", "ProjectFinanceID", financeTransaction.ProjectFinanceID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name", financeTransaction.ProjectID);
            return View(financeTransaction);
        }

        // GET: FinanceTransactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FinanceTransaction financeTransaction = db.FinanceTransactions.Find(id);
            if (financeTransaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerCNIC", "CustomerFullName", financeTransaction.CustomerID);
            ViewBag.ProjectFinanceID = new SelectList(db.ProjectFinances, "ProjectFinanceID", "ProjectFinanceID", financeTransaction.ProjectFinanceID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name", financeTransaction.ProjectID);
            return View(financeTransaction);
        }

        // POST: FinanceTransactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FinanceTransactionID,TransactionType,IdentificationDocumentNumber,IdentificationDocumentType,Amount,SenderIdentification,ReceiverIdentification,TransactionDate,ProjectFinanceID,ProjectID,CustomerID")] FinanceTransaction financeTransaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(financeTransaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerCNIC", "CustomerFullName", financeTransaction.CustomerID);
            ViewBag.ProjectFinanceID = new SelectList(db.ProjectFinances, "ProjectFinanceID", "ProjectFinanceID", financeTransaction.ProjectFinanceID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name", financeTransaction.ProjectID);
            return View(financeTransaction);
        }

        // GET: FinanceTransactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FinanceTransaction financeTransaction = db.FinanceTransactions.Find(id);
            if (financeTransaction == null)
            {
                return HttpNotFound();
            }
            return View(financeTransaction);
        }

        // POST: FinanceTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FinanceTransaction financeTransaction = db.FinanceTransactions.Find(id);
            db.FinanceTransactions.Remove(financeTransaction);
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

        public ActionResult GetFinanceList()
        {
            datatableModel model = new datatableModel();
            model.data = new List<string[]>();

            DataProvider provider = new DataProvider();
            var list = provider.GetFinanceList();
            foreach (var item in list)
            {
                model.data.Add(new string[] { item.Project.Name, item.TransactionType, item.Amount.ToString(), item.TransactionDate.ToShortDateString(), "<a href='/FinanceTransactions/Edit/" + item.FinanceTransactionID + "'>Edit</a> | <a href='/FinanceTransactions/Details/" + item.FinanceTransactionID + "'>Details</a> | <a href='/FinanceTransactions/Delete/" + item.FinanceTransactionID + "'>Delete</a>" });
            }
            return Json(model, JsonRequestBehavior.AllowGet);

        }
    }
}
