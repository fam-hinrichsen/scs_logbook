using SCS_Lookbock.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SCS_Lookbock
{
    public class Logbock : IDisposable
    {
        Dictionary<Type, Form> views = new Dictionary<Type, Form>();
        bool loggedIn;

        public Logbock(MainView mainView)
        {
            addView(mainView);
            Login();
        }

        public void AbortLogin()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            while(views.Count > 0)
            {
                closeView(views.First().Key);
            }
        }

        public void Login()
        {
            if (loggedIn)
            {
                Logout();
            }

            Login login = (Login)getView(typeof(Login));

            if (login == null)
            { 
                login = new Login(this);
                addView(login);
                login.MdiParent = getView(typeof(MainView));
            }

            login.Show();
        }

        public bool Login(string username, string password)
        {
            MainView mainView = (MainView)getView(typeof(MainView));
            Login login = (Login)getView(typeof(Login));

            mainView.updateUser(username);
            closeView(login.GetType());
            loggedIn = true;
            return true;
        }

        public bool Logout()
        {
            ((MainView)getView(typeof(MainView))).updateUser(string.Empty);
            loggedIn = false;
            return true;
        }

        private bool addView(Form view)
        {
            if (views.ContainsKey(view.GetType()))
            {
                return false;
            }

            views.Add(view.GetType(), view);
            return true;
        }

        private bool closeView(Type name)
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
