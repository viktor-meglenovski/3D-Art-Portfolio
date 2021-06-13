using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(_3D_Art_Portfolio.Startup))]
namespace _3D_Art_Portfolio
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
