using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Data;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;

namespace __NAME__.DataAccess
{
    public interface ISessionSource
    {
        ISession CreateSession();
        void BuildSchema();
    }

    public class NHibernateSessionSource : ISessionSource
    {
        private readonly ISessionFactory _factory;
        private readonly Configuration _configuration;

        public NHibernateSessionSource()
        {
            _configuration = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(csb => csb.FromConnectionStringWithKey("Main")))
                //.Mappings(mc => mc.FluentMappings.AddFromAssemblyOf<Entity>())
                .BuildConfiguration();

            _factory = _configuration.BuildSessionFactory();
        }

        public ISession CreateSession()
        {
            return _factory.OpenSession();
        }

        public void BuildSchema()
        {
            ISession session = CreateSession();
            IDbConnection connection = session.Connection;

            var dialect = new MsSql2008Dialect();
            string[] drops = _configuration.GenerateDropSchemaScript(dialect);
            ExecuteScripts(drops, connection);

            string[] scripts = _configuration.GenerateSchemaCreationScript(dialect);
            ExecuteScripts(scripts, connection);
        }

        private static void ExecuteScripts(string[] scripts, IDbConnection connection)
        {
            foreach (string script in scripts)
            {
                IDbCommand command = connection.CreateCommand();
                command.CommandText = script;
                command.ExecuteNonQuery();
            }
        }

    }
}
