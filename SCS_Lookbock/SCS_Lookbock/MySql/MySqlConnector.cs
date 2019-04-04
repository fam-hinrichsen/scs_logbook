using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCS_Lookbock.MySql
{
    public sealed class MySqlConnector : IDisposable
    {
        private static MySqlConnector instance = null;
        private static readonly object padlock = new object();

        private readonly MySqlConnection connection;
        private MyDbContext dbContext = null;
        private MySqlTransaction transaction = null;

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
            string connectionString = "server=localhost;port=3306;database=scs_logbook;uid=scs_logbook;password=test";
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

                transaction = connection.BeginTransaction();
                dbContext.Database.UseTransaction(transaction);
                return true;
            }
            catch(Exception ex)
            {
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
                transaction = null;
                return true;
            }
            catch (Exception ex)
            {
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
