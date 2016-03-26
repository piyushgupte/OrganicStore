using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OrganicStore.Startup))]
namespace OrganicStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
