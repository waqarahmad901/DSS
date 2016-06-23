using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DSS_CIO.Startup))]
namespace DSS_CIO
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
