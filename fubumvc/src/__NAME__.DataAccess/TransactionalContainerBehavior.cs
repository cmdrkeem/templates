using System;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using FubuMVC.StructureMap;
using StructureMap;

namespace __NAME__.DataAccess
{
    public class TransactionalContainerBehavior : IActionBehavior
    {
        private readonly IContainer _container;
        private readonly ServiceArguments _arguments;
        private readonly Guid _behaviorId;

        public TransactionalContainerBehavior(IContainer container, ServiceArguments arguments, Guid behaviorId)
        {
            _container = container;
            _arguments = arguments;
            _behaviorId = behaviorId;
        }

        public void Invoke()
        {
            using(var nested = _container.GetNestedContainer())
            {
                var transaction = nested.GetInstance<ITransactionBoundary>();
                try
                {
                    transaction.Start();
                    invokeRequestedBehavior(nested);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }

        public void InvokePartial()
        {
            
        }

        private void invokeRequestedBehavior(IContainer c)
        {
            var behavior = c.GetInstance<IActionBehavior>(_arguments.ToExplicitArgs(), _behaviorId.ToString());
            behavior.Invoke();
        }
    }
}


