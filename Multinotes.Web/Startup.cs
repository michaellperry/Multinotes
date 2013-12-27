using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Multinotes.Web.Startup))]
namespace Multinotes.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
