using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KSUCorner.Startup))]
namespace KSUCorner
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
