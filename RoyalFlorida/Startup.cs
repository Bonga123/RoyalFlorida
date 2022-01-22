using Microsoft.Owin;
using Owin;
using RoyalFlorida.Models;
[assembly: OwinStartupAttribute(typeof(RoyalFlorida.Startup))]
namespace RoyalFlorida
{
    public partial class Startup
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRole();
            CreateUser();
        }
    }
}
