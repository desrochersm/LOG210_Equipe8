using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Restaurents.Startup))]
namespace Restaurents
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
