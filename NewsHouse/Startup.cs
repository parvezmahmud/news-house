using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NewsHouse.Startup))]
namespace NewsHouse
{
    public partial class Startup
    {
        
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
