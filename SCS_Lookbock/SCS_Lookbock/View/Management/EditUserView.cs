using SCS_Lookbock.MySql;
using SCS_Lookbock.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCS_Lookbock.View.Management
{
    public partial class EditUserView : Form
    {
        private readonly User user;
        private readonly Form parent;

        public EditUserView(User user, Form parent)
        {
            this.user = user;
            this.parent = parent;

            InitializeComponent();
            tb_id.Text = user.Id.ToString();
            tb_username.Text = user.Username;
            tb_password.Text = user.Password;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            user.Username = tb_username.Text;
            user.Password = tb_password.Text;
            MySqlConnector.Instance.GetDbContext().SaveChanges();
            MySqlConnector.Instance.EndTransaction();
            parent.Enabled = true;
            Logbook.Instance.closeView(GetType());
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            MySqlConnector.Instance.RollbackTransaction();
            parent.Enabled = true;
            Logbook.Instance.closeView(GetType());
        }

        private void tb_username_TextChanged(object sender, EventArgs e)
        {
            if (!MySqlConnector.Instance.GetDbContext().Users.Where(user => user.Username.Equals(tb_username.Text)).Any() || tb_username.Text == user.Username)
            {
                tb_username.BackColor = Color.LightGreen;
                btn_save.Enabled = true;
            }
            else
            {
                tb_username.BackColor = Color.Red;
                btn_save.Enabled = false;
            }
        }
    }
}
