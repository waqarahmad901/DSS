using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSS_CIO.Models;

namespace DataAccess
{
    public class DataProvider
    {
        DSS_CIO2Entities context = new DSS_CIO2Entities();
        public string VerifyUserLogin(string userNamr, string password)
        {
            var user = context.People.Where(x => x.UserName == userNamr && x.Password == password).FirstOrDefault();
            if (user == null)
                return "";
            else
                return
                    user.JobDescription != null ? user.JobDescription.JobTitle : "";

        }

        public Person GetPersonByUserName(string userName)
        {
            return context.People.Where(x => x.UserName == userName).FirstOrDefault();
        }

        public List<Project> GetProjectList()
        {
            return context.Projects.ToList();

        }
        public Project GetProjectById(int projectId)
        {
            return context.Projects.Where(x => x.ProjectID == projectId).FirstOrDefault();
        }
        public List<Person> GetPersonList(string status)
        {
            return context.People.Where(x => status == "" || x.EmploymentStatus == status).ToList();
        }
        public List<InanimateResource> GetResourceList()
        {
            return context.InanimateResources.ToList();
        }

        public IList<ProjectEmployeeAssignment> GetEmployeeWorkingProjects(int id)
        {
            return context.ProjectEmployeeAssignments.Include("Person").Where(x => x.ProjectID == id).ToList();
        }

        public IList<Complaint> GetCompliantInProject(int id)
        {
            return context.Complaints.Include("Customer").Include("Deliverable").Include("Milestone").Where(x => x.ProjectID == id).ToList();
        }

        public IList<ProjectResourceAssignment> GetResourcesInProject(int id)
        {
            return context.ProjectResourceAssignments.Include("InanimateResource").Where(x => x.ProjectID == id).ToList();

        }

        public IList<EmployeeTaskAssignment> GetEmployeeTaskAssignment()
        {
            return context.EmployeeTaskAssignments.ToList();

        }

        public bool AddProjectEmployeeAssignment(ProjectEmployeeAssignment model)
        {
            context.ProjectEmployeeAssignments.Add(model);
            return context.SaveChanges() > 0;
        }

        public bool updatePerson(Person person)
        {
            //context.Entry(person).State = EntityState.Modified;
            return context.SaveChanges() > 0;
        }

        public List<ProjectResourceAssignment> GetProjectResourceAssignments()
        {
            return context.ProjectResourceAssignments.ToList();
        }

        public bool AddProjectResourceAssignment(ProjectResourceAssignment model)
        {
            context.ProjectResourceAssignments.Add(model);
            return context.SaveChanges() > 0;
        }

        public IList<EmployeeResourceAssignment> GetEmployeeResourceAssignments()
        {
            return context.EmployeeResourceAssignments.ToList();
        }

        public IList<ProjectEmployeeAssignment> GetProjectsByEmployeeId(string id)
        {
            return context.ProjectEmployeeAssignments.Where(x => x.EmployeeID == id).ToList();
        }

        public IList<EmployeeResourceAssignment> GetResourcesByEmployeeId(string id)
        {
            return context.EmployeeResourceAssignments.Where(x => x.EmployeeID == id).ToList();
        }

        public IList<FinanceTransaction> GetFinanceList()
        {
            return context.FinanceTransactions.ToList();
        }

        public FinanceTransaction GetFinanceDetailById(int id)
        {
            return context.FinanceTransactions.Where(x=>x.FinanceTransactionID == id).FirstOrDefault();
        }

        public IList<ProjectResourceAssignment> GetProjectsByResourceId(int resourceId)
        {
            return context.ProjectResourceAssignments.Where(x => x.ResourceID == resourceId).ToList();
        }

        public IList<EmployeeResourceAssignment> GetEmployeeByResourceId(int id)
        {
            return context.EmployeeResourceAssignments.Where(x => x.ResourceID == id).ToList();
        }

        public InanimateResource GetResourceById(int id)
        {
            return context.InanimateResources.FirstOrDefault(x => x.ResourceID == id);
        }

        public Person getPersonById(string id)
        {
            return context.People.Where(x => x.CNIC == id).FirstOrDefault();
        }

        public IList<Certification> GetCertificationByEmployeeId(string id)
        {
            return context.Certifications.Where(x => x.PersonID == id).ToList();
        }

        public IList<Education> GetEducationByEmployeeId(string id)
        {
            return context.Educations.Where(x => x.PersonID == id).ToList();
        }

        public List<PersonRelationshipAssignment> GetRelationShip1ByEmployeeId(string id)
        {
            return context.PersonRelationshipAssignments.Where(x => x.Person1ID == id).ToList();
        }

        public List<PersonRelationshipAssignment> GetRelationShip2ByEmployeeId(string id)
        {
            return context.PersonRelationshipAssignments.Where(x => x.Person2ID == id).ToList();
        }

        public string[] GetSaleryList(string id)
        {
            ObjectParameter  salary1 = new ObjectParameter("salary1", typeof(String));
            ObjectParameter  salary2 = new ObjectParameter("salary2", typeof(String));
            ObjectParameter  salary3 = new ObjectParameter("salary3", typeof(String));
            var result = context.KNNALGORITHM(id, salary1, salary2, salary3);
            return new string[] { salary1.Value.ToString(), salary2.Value.ToString(), salary3.Value.ToString() };

        }

        public List<SkillPrediction> GetPredectionList(string id)
        {
            var result = context.KNNALGORITHMSKILL(id);
            return context.SkillPredictions.ToList();

        }
    }
}
