using System.Web.Routing;
using __NAME__.DataAccess;
using FubuMVC.Core.Runtime;
using FubuMVC.StructureMap;
using NHibernate;
using StructureMap;

namespace __NAME__
{
    public class FubuStructureMapBootstrapper : IBootstrapper
    {
        private readonly RouteCollection _routes;

        private FubuStructureMapBootstrapper(RouteCollection routes)
        {
            _routes = routes;
            
        }

        public void BootstrapStructureMap()
        {
            UrlContext.Reset();

            ObjectFactory.Initialize(x=>
            {
                x.ForSingletonOf<ISessionSource>().Use<NHibernateSessionSource>();

                x.For<ISession>().Use(c => c.GetInstance<ISessionSource>().CreateSession());
                x.For<ITransactionBoundary>().Use<NHibernateTransactionBoundary>();
                x.For<ITransactionProcessor>().Use<TransactionProcessor>();
            });

            BootstrapFubu(ObjectFactory.Container, _routes);
        }

        public static void BootstrapFubu(IContainer container, RouteCollection routes)
        {
            var bootstrapper = new StructureMapBootstrapper(container, new __NAME__Registry());
            bootstrapper.Builder = (c, args, id) =>
            {
                return new TransactionalContainerBehavior(c, args, id);
            }; 

            bootstrapper.Bootstrap(routes);
        }

        public static void Bootstrap(RouteCollection routes)
        {
            new FubuStructureMapBootstrapper(routes).BootstrapStructureMap();
        }
    }
}
