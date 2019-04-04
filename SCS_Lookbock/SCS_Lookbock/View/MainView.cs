using System;
using System.Windows.Forms;

namespace SCS_Lookbock.View
{
    public partial class MainView : Form
    {
        bool closing;

        public MainView()
        {
            InitializeComponent();
            Logbook.Instance.AddView(this);
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logbook.Instance.Logout();
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logbook.Instance.Login();
        }

        public void updateUser(string username)
        {
            toolStripStatusLabel_user.Text = "User: " + username;
        }

        public void UpdateDbConnectionState(string state)
        {
            toolStripStatusLabel_DB.Text = "DB: " + state;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logbook.Instance.Dispose();
        }

        private void liveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void MainView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (closing)
            {
                return;
            }

            closing = true;
            e.Cancel = true;
            Logbook.Instance.Dispose();
        }

        private void userToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logbook.Instance.ListUser();
        }

        private void jobToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logbook.Instance.ListJob();
        }
    }
}
