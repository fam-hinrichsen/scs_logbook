using SCS_Lookbock.View;
using System;

namespace SCS_Lookbock
{
    public class Logbock
    {
        MainView mainView;
        Login login;

        public Logbock(MainView mainView)
        {
            this.mainView = mainView;
            Login();
        }

        public void AbortLogin()
        {
            throw new NotImplementedException();
        }

        public void Login()
        {
            login = new Login(this);
            login.MdiParent = mainView;
            login.Show();
        }

        public bool Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public bool Logout()
        {
            throw new NotImplementedException();
        }
    }
}
