using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using DataAccess;
using DSS_CIO.Models;
using DSSCIO.Areas.Test.Models;

namespace DSS_CIO.Areas.Test.Controllers
{
    [AuthorizeUser(AccessLevel = "cio")]
    public class MyController : Controller
    {
        // GET: Test/My
        public ActionResult Index()
        {

            DataProvider provider = new DataProvider();
            var list = provider.GetProjectList();


            //projects
            int projectStartedThisMonth = list.Where(x => x.LaunchDate.Month == DateTime.Now.Month).Count();
            int projectEndedThisMonth = list.Where(x => x.ActualEndDate != null && x.ActualEndDate.Value.Month == DateTime.Now.Month).Count();
            int projectDelayedThisMonth = list.Where(x => x.ActualEndDate != null && x.ActualEndDate.Value.Date > x.EcpectedEndDate.Date && x.ActualEndDate.Value.Month == DateTime.Now.Month).Count();

            int projectWithNoProfit = list.Where(x => x.ProjectFinance != null && x.ProjectFinance.ProjectBidPrice == x.ProjectFinance.ProjectResourcesAmount).Count();
            int projectWithProfit = list.Where(x => x.ProjectFinance != null && x.ProjectFinance.ProjectBidPrice > x.ProjectFinance.ProjectResourcesAmount).Count();
            int projectWithLost = list.Where(x => x.ProjectFinance != null && x.ProjectFinance.ProjectBidPrice < x.ProjectFinance.ProjectResourcesAmount).Count();

            //For employees
            var task = provider.GetEmployeeTaskAssignment();
            int beforeTimeThisMonth = task.Where(x => x.Deadline < x.SubmissionDate && x.SubmissionDate.Value.Month == DateTime.Now.Month).Count();
            int ExtendliyDeadLine = task.Where(x => x.Deadline > x.SubmissionDate && x.SubmissionDate.Value.Month == DateTime.Now.Month).Count();
            int ontimeThisMonth = task.Where(x => x.Deadline == x.SubmissionDate && x.SubmissionDate.Value.Month == DateTime.Now.Month).Count();

            //For Resources 
            var projectResources = provider.GetProjectResourceAssignments();
            int utlizedResources = projectResources.Where(x => !x.Status.ToLower().Equals("free")).Count();
            int UnUtlizedResources = projectResources.Where(x => x.Status.ToLower().Equals("free")).Count();

            var employeeResources = provider.GetEmployeeResourceAssignments();
            int emputlizedResources = employeeResources.Where(x => !x.Status.ToLower().Equals("free")).Count();
            int empUnUtlizedResources = employeeResources.Where(x => x.Status.ToLower().Equals("free")).Count();

            var rsList = provider.GetResourceList();
            int newResources = rsList.Where(x => x.Condition.Equals("New")).Count();
            int badResources = rsList.Where(x => x.Condition.Equals("Bad")).Count();
            int goodResources = rsList.Where(x => x.Condition.Equals("Good")).Count();

            var flist = provider.GetFinanceList();

            ProjectModel model = new ProjectModel();
            model.Piechart1 = new[] { projectStartedThisMonth, projectDelayedThisMonth, projectEndedThisMonth };
            model.Piechart2 = new[] { projectWithNoProfit, projectWithProfit, projectWithLost };
            model.Piechart3 = new[] { beforeTimeThisMonth, ExtendliyDeadLine, ontimeThisMonth };
            model.Piechart4 = new[] { utlizedResources, UnUtlizedResources };
            model.Piechart5 = new[] { emputlizedResources, empUnUtlizedResources };
            model.Piechart6 = new[] { newResources, badResources, goodResources };
            model.Piechart7 = new[] { newResources, badResources, goodResources }; // resouce chart 

            model.projectsCompletedToday = list.Where(x => x.ActualEndDate > DateTime.Now).Count();
            model.projectsHavinyTodayDeadline = list.Where(x => x.ActualEndDate > DateTime.Now).Count();
            model.projectsExcedingTodayDeadline = list.Where(x => x.ActualEndDate > DateTime.Now).Count();

            return View(model);
        }

        public ActionResult GetProjectList()
        {
            datatableModel model = new datatableModel();
            model.data = new List<string[]>();

            DataProvider provider = new DataProvider();
            var list = provider.GetProjectList();
            foreach (var item in list)
            {
                model.data.Add(new string[] { item.Name, item.ProjectStatus, item.EcpectedEndDate.ToString("dd/MM/yyyy"), "<a href='/Test/My/ProjectDetail/" + item.ProjectID + "'><i class='fa fa-eye'></i></a>" });
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProjectDetail(int id)
        {
            ProjectDetailModel model = new ProjectDetailModel();
            model.ProjectId = id;
            DataProvider provider = new DataProvider();
            var project = provider.GetProjectById(id);
            model.Name = project.Name;
            model.PeopleCount = project.PeopleCount.ToString();
            model.LanchDate = project.LaunchDate.ToShortDateString();
            model.ExpectedEndDate = project.EcpectedEndDate.ToShortDateString();
            model.TotalMileStone = project.TotalMilestones.Value;
            model.ProjStatus = project.ProjectStatus;
            return View(model);
        }

        public ActionResult ProjectEmployeementDetail(int ProjectId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            DataProvider provider = new DataProvider();
            var list = provider.GetPersonList("Employee");
            foreach (var item in list)
            {
                items.Add(new SelectListItem { Text = item.FName + " " + item.LName, Value = item.CNIC });
            }
            ProjectEmployee model = new ProjectEmployee();
            ViewBag.employee = items;


            return View(model);
        }
        [HttpPost]
        public ActionResult ProjectEmployeementDetail(ProjectEmployee model)
        {
            ProjectEmployeeAssignment pe = new ProjectEmployeeAssignment();
            pe.AppointmentDate = model.AppointmentDate;
            pe.ProjectID = model.ProjectId;
            pe.EmployeeID = model.CNIC;
            pe.Role = model.Role;
            pe.Performance = model.performance;

            DataProvider provider = new DataProvider();
            provider.AddProjectEmployeeAssignment(pe);

            return RedirectToAction("ProjectDetail", new { id = model.ProjectId });

        }

        public ActionResult ProjectResourcesDetail(int ProjectId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            DataProvider provider = new DataProvider();
            var list = provider.GetResourceList();
            foreach (var item in list)
            {
                items.Add(new SelectListItem { Text = item.Name, Value = item.ResourceID.ToString() });
            }
            ProjectEmployee model = new ProjectEmployee();
            ViewBag.employee = items;

            return View();
        }
        [HttpPost]
        public ActionResult ProjectResourcesDetail(ProjectResources model)
        {
            ProjectResourceAssignment pr = new ProjectResourceAssignment();
            pr.ProjectID = model.ProjectId;
            pr.ResourceID = model.ResourceId;
            pr.Status = model.Status;
            pr.AssignmentDate = model.AppoinmentDate;
            pr.ActualReturnDate = model.ActualReturnDate;
            pr.ExpectedReturnDate = model.ExpectedReturnDate;

            DataProvider provider = new DataProvider();
            provider.AddProjectResourceAssignment(pr);

            return RedirectToAction("ProjectDetail", new { id = model.ProjectId });
        }

        public ActionResult ProjectComplaintDetail(int ProjectId)
        {

            return View();
        }

        public ActionResult GetEmployeeWorkingProjects(int id)
        {
            datatableModel model = new datatableModel();
            model.data = new List<string[]>();

            DataProvider provider = new DataProvider();
            var list = provider.GetEmployeeWorkingProjects(id);
            foreach (var item in list)
            {
                model.data.Add(new string[] { item.Person.FName + " " + item.Person.LName, item.Role, item.AppointmentDate == null ? "" : item.AppointmentDate.Value.ToShortDateString(), item.Performance });
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCompliantInProjects(int id)
        {
            datatableModel model = new datatableModel();
            model.data = new List<string[]>();

            DataProvider provider = new DataProvider();
            var list = provider.GetCompliantInProject(id);
            foreach (var item in list)
            {
                model.data.Add(new string[] { item.Customer.CustomerFullName, item.Category, "", item.Description, item.Severity.Value.ToString() });
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetResourcesInProjects(int id)
        {
            datatableModel model = new datatableModel();
            model.data = new List<string[]>();

            DataProvider provider = new DataProvider();
            var list = provider.GetResourcesInProject(id);
            foreach (var item in list)
            {
                model.data.Add(new string[] { item.InanimateResource.Name, item.Status, item.AssignmentDate.ToShortDateString(), item.ExpectedReturnDate == null ? "" : item.ExpectedReturnDate.Value.ToShortDateString(), item.ActualReturnDate == null ? "" : item.ActualReturnDate.Value.ToShortDateString() });
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmployeeList()
        {


            datatableModel model = new datatableModel();
            model.data = new List<string[]>();

            DataProvider provider = new DataProvider();
            var list = provider.GetPersonList("Employee");
            foreach (var item in list)
            {
                model.data.Add(new string[] { item.FName + " " + item.LName, item.JobDescription.JobTitle, item.ProjectEmployeeAssignments.GroupBy(x => x.ProjectID).Select(x => x).Count().ToString(), item.City, "<a href='/Test/My/EmployeeDetail/" + item.CNIC + "'><i class='fa fa-eye'></i></a>" });
            }
            return Json(model, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetCandidateList()
        {
            datatableModel model = new datatableModel();
            model.data = new List<string[]>();

            DataProvider provider = new DataProvider();
            var list = provider.GetPersonList("Candidate");
            //foreach (var item in list)
            //{
            //    model.data.Add(new string[] { item.FName + " " + item.LName, item.City, "<a href='/Test/My/EmployeeDetail/" + item.CNIC.Trim() + "?candidate=yes'><i class='fa fa-eye'></i></a>" });
            //}
            model.data.Add(new string[] { "JAN - 2016", "45000","" });
            model.data.Add(new string[] { "FEB - 2016", "50000","" });
            model.data.Add(new string[] { "MAR - 2016", "45000" ,""});
            model.data.Add(new string[] { "APRIL - 2016", "50000","" });
            model.data.Add(new string[] { "MAY - 2016", "45000","" }); 
            return Json(model, JsonRequestBehavior.AllowGet);

        }


        public ActionResult GetFinanceList()
        {
            datatableModel model = new datatableModel();
            model.data = new List<string[]>();

            DataProvider provider = new DataProvider();
            var list = provider.GetFinanceList();
            foreach (var item in list)
            {
                model.data.Add(new string[] { item.Project.Name, item.TransactionType, item.Amount.ToString(), item.TransactionDate.ToShortDateString(), "<a href='/Test/My/FinanceDetail/" + item.FinanceTransactionID + "'><i class='fa fa-eye'></i></a>" });
            }
            return Json(model, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetResourceList()
        {
            datatableModel model = new datatableModel();
            model.data = new List<string[]>();

            DataProvider provider = new DataProvider();
            var list = provider.GetResourceList();
            foreach (var item in list)
            {
                model.data.Add(new string[] { item.Name, item.Model, item.Make, item.Price.ToString(), item.Condition, "<a href='/Test/My/ResourceDetail/" + item.ResourceID + "'><i class='fa fa-eye'></i></a>" });
            }
            return Json(model, JsonRequestBehavior.AllowGet);

        }

        public ActionResult FinanceDetail(int id)
        {
            DataProvider provider = new DataProvider();
            var ft = provider.GetFinanceDetailById(id);
            return View(ft);
        }

        public ActionResult ResourceDetail(int id)
        {
            DataProvider provider = new DataProvider();
            var ft = provider.GetResourceById(id);
            return View(ft);
        }

        public ActionResult EmployeeDetail(string id)
        {
            DataProvider provider = new DataProvider();
            var person = provider.GetPersonList("").Where(x => x.CNIC.Trim() == id).FirstOrDefault();
            return View(person);
        }

        public ActionResult GetProjectsByEmployeeId(string id)
        {
            datatableModel model = new datatableModel();
            model.data = new List<string[]>();

            DataProvider provider = new DataProvider();
            var list = provider.GetProjectsByEmployeeId(id);
            //foreach (var item in list)
            //{
            //    model.data.Add(new string[] { item.Project.Name, item.Role, item.AppointmentDate == null ? "" : item.AppointmentDate.Value.ToShortDateString(), item.Performance });
            //}
                model.data.Add(new string[] { "1-1-2016", "0930", "1830", "12h"});
                model.data.Add(new string[] { "1-1-2016", "0930", "1530", "9h"});
                model.data.Add(new string[] { "1-1-2016", "0930", "1730", "11h"});
                model.data.Add(new string[] { "1-1-2016", "0830", "1730", "12h"});
                model.data.Add(new string[] { "1-1-2016", "0600", "1200", "6h"});
                model.data.Add(new string[] { "1-2-2016", "0930", "1830", "12h"});
                model.data.Add(new string[] { "1-3-2016", "0930", "1830", "12h"});
                model.data.Add(new string[] { "1-4-2016", "0930", "1830", "12h"});

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetResourcesByEmployeeId(string id)
        {
            datatableModel model = new datatableModel();
            model.data = new List<string[]>();

            DataProvider provider = new DataProvider();
            var list = provider.GetResourcesByEmployeeId(id);
            //foreach (var item in list)
            //{
            //    model.data.Add(new string[] { item.InanimateResource.Name, item.Status, item.AssignmentDate.ToShortDateString(), item.ExpectedReturnDate == null ? "" : item.ExpectedReturnDate.Value.ToShortDateString(), item.ActualReturnDate == null ? "" : item.ActualReturnDate.Value.ToShortDateString() });
            //}
            model.data.Add(new string[] { "JAN - 2016", "25", "1d1h", "15000.00", "15000.00", "0.00","15000.00"});
            model.data.Add(new string[] { "FEB - 2016", "25", "1d2h", "14500.00", "14500.00", "0.00","14500.00"});
            model.data.Add(new string[] { "MARCH - 2016", "25", "1d1h", "15000.00", "15000.0" , "0.00","15000.00" });
            model.data.Add(new string[] { "APRL - 2016", "25", "1d1h", "15000.00", "15000.00" , "0.00","15000.00" });
            model.data.Add(new string[] { "MAY - 2016", "25", "1d1h", "15000.00", "15000.00" , "0.00","15000.00"});
            model.data.Add(new string[] { "JUN - 2016", "25", "1d1h", "15000.00", "15000.00" , "0.00","15000.00"});
            model.data.Add(new string[] { "JULY - 2016", "25", "1d1h", "15000.00", "15000.00" , "0.00","15000.00" });

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCertificationByEmployeeId(string id)
        {
            datatableModel model = new datatableModel();
            model.data = new List<string[]>();

            DataProvider provider = new DataProvider();
            var list = provider.GetCertificationByEmployeeId(id);
            foreach (var item in list)
            {
                model.data.Add(new string[] { item.Name, item.Duration, item.CompletionStatus, item.Grade, item.ApplicationStatus });
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetEducationByEmployeeId(string id)
        {
            datatableModel model = new datatableModel();
            model.data = new List<string[]>();

            DataProvider provider = new DataProvider();
            var list = provider.GetEducationByEmployeeId(id);
            foreach (var item in list)
            {
                model.data.Add(new string[] { item.DegreeName, item.DegreeType, item.Board_University, item.CGPA_Percentage.ToString(), item.Awards, item.CompletionStatus });
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSaleryList(string id)
        {
            datatableModel model = new datatableModel();
            model.data = new List<string[]>();

            DataProvider provider = new DataProvider();
            var arr = provider.GetSaleryList(id);
            model.data.Add(new string[] { arr[0], arr[1], arr[2] });

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPredectionList(string id)
        {
            datatableModel model = new datatableModel();
            model.data = new List<string[]>();

            DataProvider provider = new DataProvider();
            var arr = provider.GetPredectionList(id);
            foreach (var item in arr)
            {
                model.data.Add(new string[] { item.Skill,item.Performance,item.Efficiency });
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetRelationshipByEmployeeId(string id)
        {
            datatableModel model = new datatableModel();
            model.data = new List<string[]>();

            DataProvider provider = new DataProvider();
            var list = provider.GetRelationShip1ByEmployeeId(id);
            var list2 = provider.GetRelationShip2ByEmployeeId(id);
            foreach (var item in list2)
            {
                model.data.Add(new string[] { item.Person.FName + " " + item.Person.LName, item.Relationship });
            }
            foreach (var item in list)
            {
                model.data.Add(new string[] { item.Person1.FName + " " + item.Person1.LName, item.Relationship2 });
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProjectsByResourcesId(int id)
        {
            datatableModel model = new datatableModel();
            model.data = new List<string[]>();

            DataProvider provider = new DataProvider();
            var list = provider.GetProjectsByResourceId(id);
            foreach (var item in list)
            {
                model.data.Add(new string[] { item.Project.Name, item.Status, item.AssignmentDate.ToShortDateString(), item.ExpectedReturnDate == null ? "" : item.ExpectedReturnDate.Value.ToShortDateString(), item.ActualReturnDate == null ? "" : item.ActualReturnDate.Value.ToShortDateString() });
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmployeeUsingResources(int id)
        {
            datatableModel model = new datatableModel();
            model.data = new List<string[]>();

            DataProvider provider = new DataProvider();
            var list = provider.GetEmployeeByResourceId(id);
            foreach (var item in list)
            {
                model.data.Add(new string[] { item.Person.FName + " " + item.Person.LName, item.Status, item.AssignmentDate.ToShortDateString(), item.ExpectedReturnDate == null ? "" : item.ExpectedReturnDate.Value.ToShortDateString(), item.ActualReturnDate == null ? "" : item.ActualReturnDate.Value.ToShortDateString() });
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }



        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetEmployeeForRegister(string id)
        {
            DataProvider provider = new DataProvider();
            var person = provider.getPersonById(id) == null ? new Person() : provider.getPersonById(id);
            var per = new { Name = person.FName + " " + person.LName, Status = person.EmploymentStatus, description = person.JobDescription == null ? "" : person.JobDescription.JobTitle };
            return Json(per, JsonRequestBehavior.AllowGet);
        }


    }
}