using System.Web.Mvc;

namespace DSS_CIO.Areas.Test
{
    public class TestAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Test";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Test_default",
                "Test/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "DSS_CIO.Areas.Test.Controllers" }
            );
        }
    }
}