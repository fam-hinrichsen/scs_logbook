using log4net;
using SCS_Logbook.MySql;
using SCS_Logbook.Objects;
using SCS_Logbook.Objects.Constants;
using SCS_Logbook.View;
using SCS_Logbook.View.Management.Jobmanagement;
using SCS_Logbook.View.Management.Usermanagement;
using SCSSdkClient;
using SCSSdkClient.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SCS_Logbook
{
    public class Logbook : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static Logbook instance = null;
        private static readonly object padlock = new object();
        private readonly object telemetryDataLock = new object();

        private readonly Dictionary<Type, Form> views = new Dictionary<Type, Form>();
        private User activeUser = null;
        private SCSSdkTelemetry telemetry;
        private SCSTelemetry telemetryData;
        private Job activeJob;

        public static Logbook Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Logbook();
                    }

                    return instance;
                }
            }
        }

        public void AbortLogin()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            UnloadScsSdkClient();
            Logout();

            MySqlConnector.Instance.Dispose();

            while (views.Count > 0)
            {
                closeView(views.First().Key);
            }
        }

        public void Login()
        {
            if (activeUser != null)
            {
                Logout();
            }

            Login login = (Login)getView(typeof(Login));

            if (login == null)
            { 
                login = new Login(this);
                AddView(login);
            }

            login.Show();
        }

        public void UpdateDbConnectionState(string state)
        {
            MainView view = (MainView)getView(typeof(MainView));
            view.UpdateDbConnectionState(state);
        }

        public void NewUser()
        {
            NewUserView newUser = (NewUserView)getView(typeof(NewUserView));

            if (newUser == null)
            {
                newUser = new NewUserView();
                AddView(newUser);
            }

            newUser.Show();
        }

        public void OpenLiveView()
        {
            LiveView liveView = (LiveView)getView(typeof(LiveView));

            if (liveView == null)
            {
                liveView = new LiveView();
                AddView(liveView);
            }

            liveView.Show();
        }

        public void ListUser()
        {
            UserListView users = (UserListView)getView(typeof(UserListView));

            if (users == null)
            {
                users = new UserListView(MySqlConnector.Instance.GetDbContext().Users, typeof(EditUserView), typeof(NewUserView));
                AddView(users);
            }

            users.Show();
        }

        public void ListJob()
        {
            JobListView jobs = (JobListView)getView(typeof(JobListView));

            if (jobs == null)
            {
                jobs = new JobListView(MySqlConnector.Instance.GetDbContext().Jobs, typeof(EditJobView), typeof(NewJobView));
                AddView(jobs);
            }

            jobs.Show();
        }

        public bool Login(string username, string password)
        {
            try { 
                MainView mainView = (MainView)getView(typeof(MainView));
                Login login = (Login)getView(typeof(Login));

                User tmpUser = MySqlConnector.Instance.GetDbContext().Users.Where(user => user.Username.Equals(username)).First();
                if (tmpUser.Password.Equals(password))
                {
                    mainView.updateUser(username);
                    closeView(login.GetType());
                    activeUser = tmpUser;
                    InitScsSdkClient();
                    return true;
                }
            }
            catch(Exception ex)
            {
                log.Error("Login failed." , ex);
            }

            return false;
        }

        public bool Logout()
        {
            UnloadScsSdkClient();
            ((MainView)getView(typeof(MainView))).updateUser(string.Empty);
            activeUser = null;
            return true;
        }

        public bool AddView(Form view)
        {
            if (views.ContainsKey(view.GetType()))
            {
                return false;
            }

            views.Add(view.GetType(), view);
            if (!view.GetType().Equals(typeof(MainView)))
            {
                view.MdiParent = getView(typeof(MainView));
            }

            return true;
        }

        public bool closeView(Type name)
        {
            if (views.TryGetValue(name, out Form view))
            {
                view.Close();
                views.Remove(name);
                return true;
            }

            return false;
        }

        private Form getView(Type name)
        {
            if(!views.ContainsKey(name))
            {
                return null;
            }

            return views[name];
        }

        private bool InitScsSdkClient()
        {
            try
            {
                telemetry = new SCSSdkTelemetry();
                telemetry.Data += Telemetry_Data;
                telemetry.JobFinished += Telemetry_JobFinished;
                telemetry.JobStarted += Telemetry_JobStarted;
                //telemetry.TrailerConnected += TelemetryTrailerConnected;
                //telemetry.TrailerDisconnected += TelemetryTrailerDisconnected;
                return true;
            }
            catch(Exception ex)
            {
                log.Fatal("Cannot initialize SCS SDK client.", ex);
            }

            return false;
        }

        private bool UnloadScsSdkClient()
        {
            try
            {
                if (telemetry != null) {
                    telemetry.Data -= Telemetry_Data;
                    telemetry.JobFinished -= Telemetry_JobFinished;
                    telemetry.JobStarted -= Telemetry_JobStarted;
                    telemetry.Dispose();
                    telemetry = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                log.Fatal("Cannot unload SCS SDK client.", ex);
            }

            return false;
        }

        private void Telemetry_Data(SCSTelemetry data, bool newTimestamp)
        {
            lock (telemetryDataLock)
            {
                telemetryData = data;
                if(activeJob != null && activeJob.Distance == 0)
                {
                    MySqlConnector.Instance.BeginTransaction();
                    activeJob.Distance = data.NavigationValues.NavigationDistance;
                    MySqlConnector.Instance.GetDbContext().SaveChanges();
                    MySqlConnector.Instance.EndTransaction();
                }

                LiveView live = (LiveView)getView(typeof(LiveView));
                if(live != null)
                {
                    live.Update(telemetryData);
                }
            }
        }

        private void Telemetry_JobFinished(object sender, EventArgs e)
        {
            try
            {
                MySqlConnector.Instance.BeginTransaction();
                activeJob.IsActive = false;
                MySqlConnector.Instance.GetDbContext().SaveChanges();
                MySqlConnector.Instance.EndTransaction();
            }
            catch(Exception ex)
            {
                log.Error("Could not finish job from SDK.", ex);
                MySqlConnector.Instance.RollbackTransaction();
            }
        }

        private void Telemetry_JobStarted(object sender, EventArgs e)
        {
            lock (telemetryDataLock)
            {
                try
                {
                    MySqlConnector.Instance.BeginTransaction();

                    activeJob = MySqlConnector.Instance.GetDbContext().Jobs.Where(j => j.OwnerForeignKey == activeUser.Id && j.IsActive == true).FirstOrDefault();
                    
                    string cargoName = telemetryData.JobValues.CargoValues.Id;
                    Cargo cargo = MySqlConnector.Instance.GetDbContext().Cargos.Where(c => c.Name == cargoName).FirstOrDefault();
                    if (cargo == null)
                    {
                        cargo = new Cargo
                        {
                            Name = cargoName,
                            NameLocal = telemetryData.JobValues.CargoValues.Name
                        };
                        MySqlConnector.Instance.GetDbContext().Cargos.Add(cargo);
                    }

                    string citySourceName = telemetryData.JobValues.CitySourceId;
                    City citySource = MySqlConnector.Instance.GetDbContext().Cities.Where(c => c.Name == citySourceName).FirstOrDefault();
                    if (citySource == null)
                    {
                        citySource = new City
                        {
                            Name = citySourceName,
                            NameLocal = telemetryData.JobValues.CitySource
                        };
                        MySqlConnector.Instance.GetDbContext().Cities.Add(citySource);
                    }

                    string cityDestinationName = telemetryData.JobValues.CityDestinationId;
                    City cityDestination = MySqlConnector.Instance.GetDbContext().Cities.Where(c => c.Name == cityDestinationName).FirstOrDefault();
                    if (cityDestination == null)
                    {
                        cityDestination = new City
                        {
                            Name = cityDestinationName,
                            NameLocal = telemetryData.JobValues.CityDestination
                        };
                        MySqlConnector.Instance.GetDbContext().Cities.Add(cityDestination);
                    }

                    string companyDestinationName = telemetryData.JobValues.CompanyDestinationId;
                    Company companyDestination = MySqlConnector.Instance.GetDbContext().Companies.Where(c => c.Name == companyDestinationName).FirstOrDefault();
                    if (companyDestination == null)
                    {
                        companyDestination = new Company
                        {
                            Name = companyDestinationName,
                            NameLocal = telemetryData.JobValues.CompanyDestination
                        };
                        MySqlConnector.Instance.GetDbContext().Companies.Add(companyDestination);
                    }

                    string companySourceName = telemetryData.JobValues.CompanySourceId;
                    Company companySource = MySqlConnector.Instance.GetDbContext().Companies.Where(c => c.Name == companySourceName).FirstOrDefault();
                    if (companySource == null)
                    {
                        companySource = new Company
                        {
                            Name = companySourceName,
                            NameLocal = telemetryData.JobValues.CompanySource
                        };
                        MySqlConnector.Instance.GetDbContext().Companies.Add(companySource);
                    }

                    if(activeJob == null)
                    {
                        activeJob = new Job
                        {
                            Cargo = cargo,
                            CargoMass = telemetryData.JobValues.CargoValues.Mass,
                            CitySource = citySource,
                            CityDestination = cityDestination,
                            CompanyDestination = companyDestination,
                            CompanySource = companySource,
                            Distance = telemetryData.NavigationValues.NavigationDistance,
                            IsActive = true,
                            Income = telemetryData.JobValues.Income,
                            Name = telemetryData.JobValues.CargoValues.Name,
                            Owner = activeUser
                        };

                        MySqlConnector.Instance.GetDbContext().Jobs.Add(activeJob);
                        MySqlConnector.Instance.GetDbContext().SaveChanges();
                    }

                    MySqlConnector.Instance.EndTransaction();
                }
                catch(Exception ex)
                {
                    log.Error("Could not create new job from SDK.", ex);
                    MySqlConnector.Instance.RollbackTransaction();
                }
            }
        }        
    }
}
