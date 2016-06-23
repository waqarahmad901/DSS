using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DataAccess;

namespace DSS_CIO.Areas.Test.Controllers
{
    public class AccountController : Controller
    {
        // GET: Test/Account
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(FormCollection collection)
        {
            string UserName = collection["username"];
            string Password = collection["password"];
            // validate user name and pass word
            DataProvider da = new DataProvider();
            string user = da.VerifyUserLogin(UserName, Password);
            ViewBag.IsLogin = user != "" ;
            if (user != "")
            {
                
                FormsAuthentication.SetAuthCookie(UserName, true);
                if (user.ToLower().Equals("cio"))
                    return RedirectToAction("Index", "My");
                else
                    return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
                return View();
        }
        [HttpPost]
        public ActionResult Register(FormCollection collection)
        {
            string CNIC = collection["CNIC"];
            string UserName = collection["UserName"];
            string Password = collection["Password"];

            Session["FromRegister"] = true;
            var provider = new DataProvider();
            var person = provider.getPersonById(CNIC);
            person.UserName = UserName;
            person.Password = Password;
            provider.updatePerson(person);
            return RedirectToAction("Login");
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", new { area = "Test" });
        }
    }
}