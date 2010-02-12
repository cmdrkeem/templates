using System;
using System.Web;
using System.Web.Routing;
using FubuMVC.StructureMap.Bootstrap;

namespace __NAME__
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            var routeCollection = RouteTable.Routes;
            FubuStructureMapBootstrapper.Bootstrap(routeCollection);
        }
    }
}