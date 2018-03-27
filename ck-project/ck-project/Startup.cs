using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ck_project.Startup))]
namespace ck_project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
