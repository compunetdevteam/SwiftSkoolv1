using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SwiftSkoolv1.WebUI.Startup))]
namespace SwiftSkoolv1.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
