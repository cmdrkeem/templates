using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;

namespace __NAME__.DataAccess
{
    public interface ITransactionProcessor
    {

        void Execute<T>(Action<T> action);

        RETURN Execute<T, RETURN>(Func<T, RETURN> func);

        void Execute<T>(Action<T, IContainer> action);

        void Execute<T>(string instanceName, Action<T> action);

    }

    public class TransactionProcessor : ITransactionProcessor
    {

        private readonly object _locker = new object();

        private IContainer _container;

        public TransactionProcessor(IContainer container)
        {

            _container = container;

        }

        public IContainer Container
        {

            get { return _container; }

            set
            {

                lock (_locker)
                {

                    _container = value;

                }

            }

        }

        #region ITransactionProcessor Members

        public void Execute<T>(Action<T> action)
        {

            execute(c =>
            {

                var service = c.GetInstance<T>();

                action(service);

            });

        }

        public RETURN Execute<T, RETURN>(Func<T, RETURN> func)
        {

            RETURN result = default(RETURN);

            execute(c =>
            {

                var service = c.GetInstance<T>();

                result = func(service);

            });

            return result;

        }

        public void Execute<T>(Action<T, IContainer> action)
        {

            execute(c =>
            {

                var service = c.GetInstance<T>();

                action(service, c);

            });

        }

        public void Execute<T>(string instanceName, Action<T> action)
        {

            execute(c =>
            {

                var service = c.GetInstance<T>(instanceName);

                action(service);

            });

        }

        #endregion

        private void execute(Action<IContainer> action)
        {

            IContainer container = null;

            lock (_locker)
            {

                container = _container;

            }

            using (IContainer nestedContainer = container.GetNestedContainer())

            using (var boundary = nestedContainer.GetInstance<ITransactionBoundary>())
            {

                boundary.Start();

                action(nestedContainer);

                boundary.Commit();

            }

        }

    }
}
