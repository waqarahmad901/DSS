using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess;
 
namespace DSS_CIO
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        // Custom property
        public string AccessLevel { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            var person = new DataProvider().GetPersonByUserName(httpContext.User.Identity.Name); // Call another method to get rights of the user from DB

            if (person.JobDescription.JobTitle.ToLower().Contains(this.AccessLevel))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}