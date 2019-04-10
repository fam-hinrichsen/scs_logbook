using log4net;
using MySql.Data.MySqlClient;
using System;
using System.Data.Entity;

namespace SCS_Logbook.MySql
{
    public sealed class MySqlConnector : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static MySqlConnector instance = null;
        private static readonly object padlock = new object();

        private readonly MySqlConnection connection;
        private MyDbContext dbContext = null;
        private DbContextTransaction transaction = null;

        public static MySqlConnector Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new MySqlConnector();
                    }

                    return instance;
                }
            }
        }

        public MySqlConnector()
        {
            string connectionString = "server=fam-hinrichsen.de;port=3306;database=scs_logbook;uid=scs;password=aBxpvpZFyjk5K5jV2odf";
            connection = new MySqlConnection(connectionString);
            Logbook.Instance.UpdateDbConnectionState(connection.State.ToString());
            connection.StateChange += Connection_StateChange;
        }

        private void Connection_StateChange(object sender, System.Data.StateChangeEventArgs e)
        {
            Logbook.Instance.UpdateDbConnectionState(e.CurrentState.ToString());
        }

        public bool Connect()
        {
            try
            {
                if(!IsConnected())
                { 
                    connection.Open();
                }

                if(dbContext != null) { 
                    dbContext.Dispose();
                }
                dbContext = new MyDbContext(connection, false);

                return true;
            }
            catch(Exception ex)
            {
                log.Fatal("Error while connection to database.", ex);
            }

            return false;
        }

        public MyDbContext GetDbContext()
        {
            if (!IsConnected() || dbContext == null)
            {
                Connect();
            }

            return dbContext;
        }

        public bool BeginTransaction()
        {
            try
            {
                if(transaction != null)
                {
                    return false;
                }

                transaction = dbContext.Database.BeginTransaction();
                return true;
            }
            catch(Exception ex)
            {
                log.Error("Could not begin transaction", ex);

                if(transaction != null)
                {
                    transaction.Rollback();
                    transaction = null;
                }
            }

            return false;
        }

        public bool EndTransaction()
        {
            try
            {
                if (transaction == null)
                {
                    return false;
                }

                transaction.Commit();
                transaction.Dispose();
                transaction = null;
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Could not end transaction", ex);
                if (transaction != null)
                {
                    transaction.Rollback();
                    transaction = null;
                }
            }

            return false;
        }

        public bool RollbackTransaction()
        {
            try
            {
                if (transaction == null)
                {
                    return false;
                }

                transaction.Rollback();
                transaction = null;
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Could not rollback transaction.", ex);
                if (transaction != null)
                {
                    transaction.Rollback();
                    transaction = null;
                }
            }

            return false;
        }

        public void Dispose()
        {
            RollbackTransaction();
            connection.Close();
            connection.Dispose();
            instance = null;
        }

        private bool IsConnected()
        {
            if(connection.State == System.Data.ConnectionState.Open)
            {
                return true;
            }

            return false;
        }
    }
}
