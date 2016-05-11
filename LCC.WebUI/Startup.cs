using Microsoft.Owin;
using Owin;
using System.Globalization;
using System.Threading;

[assembly: OwinStartupAttribute(typeof(LCC.WebUI.Startup))]
namespace LCC.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
          
            ConfigureAuth(app);
        }
    }
}
