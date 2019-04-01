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
    }
}
