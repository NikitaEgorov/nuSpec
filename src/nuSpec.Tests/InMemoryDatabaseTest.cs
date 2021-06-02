using System;
using System.IO;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

using NHibernate;
using NHibernate.Stat;
using NHibernate.Tool.hbm2ddl;

namespace nuSpec.Tests
{
    public abstract class InMemoryDatabaseTest
    {
        protected readonly ISession Session;

        protected readonly ISessionFactory SessionFactory;

        protected InMemoryDatabaseTest()
        {
            var configuration =
                Fluently.Configure()
                        .Database(
                                  SQLiteConfiguration.Standard
                                                     .InMemory()
                                                     .Raw("sql_types.keep_datetime", "true")
                                                     .Raw("generate_statistics", "true")
                                                     .FormatSql()
                                                     .ShowSql())
                        .Diagnostics(
                                     a =>
                                     {
                                         a.OutputToConsole();
                                         a.Enable();
                                     })
                        .Mappings(
                                  c =>
                                  {
                                      c.FluentMappings
                                       .AddFromAssembly(this.GetType().Assembly);
                                  });

            this.SessionFactory = configuration.BuildSessionFactory();

            var schemaExport = new SchemaExport(configuration.BuildConfiguration());

            this.Session = this.SessionFactory.OpenSession();
            schemaExport.Execute(false, true, false, this.Session.Connection, TextWriter.Null);
        }
    }
}
