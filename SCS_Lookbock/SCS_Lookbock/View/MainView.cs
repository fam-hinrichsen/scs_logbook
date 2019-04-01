using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCS_Lookbock.View
{
    public partial class MainView : Form
    {
        Logbock logbock;
        bool closing;

        public MainView()
        {
            InitializeComponent();
            logbock = new Logbock(this);
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logbock.Logout();
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logbock.Login();
        }

        public void updateUser(string username)
        {
            toolStripStatusLabel_user.Text = username;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logbock.Dispose();
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
            logbock.Dispose();
        }
    }
}
