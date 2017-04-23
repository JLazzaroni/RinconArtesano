using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RinconArtesano.Startup))]
namespace RinconArtesano
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
