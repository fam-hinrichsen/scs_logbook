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
    public partial class Login : Form
    {
        readonly Logbock logbock;

        public Login(Logbock logbock)
        {
            InitializeComponent();
            this.logbock = logbock;
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            if(logbock.Login(tb_user.Text, tb_password.Text))
            {
            }
        }
    }
}
