using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SuperBob.Startup))]
namespace SuperBob
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
