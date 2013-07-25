using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using WebApp.Database.Entities;

namespace WebApp.Database
{

    public sealed class DatabaseModel : DbContext
    {
        private static bool compatibilityWasChecked;
        private static bool providerInitialized;

        private bool areSetsInitialized = false;

        private static readonly Mutex mutex = new Mutex();

        public DatabaseModel()
            : base(CreateConnection(), true)
        {
            mutex.WaitOne();

            InitializeSets();

            Configuration.AutoDetectChangesEnabled = true;
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;

            Configuration.ValidateOnSaveEnabled = false;

            if (!Database.Exists())
            {
                Database.CreateIfNotExists();

                SaveChanges();
            }

            CheckCompability();
        }

        private void CheckCompability()
        {
            if (compatibilityWasChecked)
                return;

            if (!Database.CompatibleWithModel(true))
            {
#if DEBUG
                Database.Delete();
                Database.Create();
#else
                throw new InvalidOperationException("Database from file has different version with the current code. Please contact technical support");
#endif
            }

            compatibilityWasChecked = true;
        }

        private void InitializeSets()
        {
            if (areSetsInitialized)
                throw new InvalidOperationException("Sets had already been initialized");

            UsersSet = Set<DbUser>();
            PointsSet = Set<DbPoint>();

            areSetsInitialized = true;
        }

        private static DbConnection CreateConnection()
        {
            if (!providerInitialized)
            {
                try
                {
                    using (var dataSet = (DataSet) System.Configuration.ConfigurationManager.GetSection("system.data"))
                    {
                        const string name = "Microsoft SQL Server Compact Data Provider";
                        const string invariant = "System.Data.SqlServerCe.4.0";
                        const string description = ".NET Framework Data Provider for Microsoft SQL Server Compact";
                        const string type =
                            "System.Data.SqlServerCe.SqlCeProviderFactory, System.Data.SqlServerCe, Version=4.0";

                        DataRowCollection rows = dataSet.Tables[0].Rows;

                        if (!rows.Contains(invariant))
                            rows.Add(name, description, invariant, type);
                    }
                }
                catch (ConstraintException)
                {
                }
                providerInitialized = true;
            }

            var folder = AppDomain.CurrentDomain.BaseDirectory + @"\Database"; //"c:\\Temp";
            var databaseFile = "questions.sdf";
            var databaseName = "questions";

            var connectionStringBuilder = new SqlConnectionStringBuilder
                {
                    DataSource = Path.Combine(folder, databaseFile),
                    Password = "Qwerty123"
                };

            var factory = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0", folder,
                                                     connectionStringBuilder.ToString());

            return factory.CreateConnection(databaseName);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            mutex.ReleaseMutex();
        }

        public DbSet<DbUser> UsersSet { get; private set; }

        public DbSet<DbPoint> PointsSet { get; private set; }
    }
}