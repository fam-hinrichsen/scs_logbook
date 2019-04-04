using SCS_Lookbock.MySql;
using SCS_Lookbock.Objects;
using SCS_Lookbock.View;
using SCS_Lookbock.View.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SCS_Lookbock
{
    public class Logbook : IDisposable
    {
        private static Logbook instance = null;
        private static readonly object padlock = new object();

        Dictionary<Type, Form> views = new Dictionary<Type, Form>();
        User activeUser = null;
        
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

        public void ListUser()
        {
            UsersView users = (UsersView)getView(typeof(UsersView));

            if (users == null)
            {
                users = new UsersView();
                AddView(users);
            }

            users.Show();
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
                return true;
            }
            }
            catch(Exception ex)
            {

            }

            return false;
        }

        public bool Logout()
        {
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
            Form view;
            if(views.TryGetValue(name,out view))
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
    }
}
