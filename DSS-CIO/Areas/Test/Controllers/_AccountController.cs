using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DataAccess;

namespace DSSCIO.Areas.Test.Controllers
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
            bool isLogin = da.VerifyUserLogin(UserName, Password);
            ViewBag.IsLogin = isLogin;
            if (isLogin)
            {
                FormsAuthentication.SetAuthCookie(UserName, true);

                return RedirectToAction("Index", "My");
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

            return RedirectToAction("Login");
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}