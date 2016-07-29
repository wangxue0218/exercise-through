using System;
using System.Web;
using System.Web.Http;
using PosApp;

namespace PosAppHost
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            new Bootstrap().Initialize(GlobalConfiguration.Configuration);
        }
    }
}