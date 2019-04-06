using SCS_Logbook.MySql;
using SCS_Logbook.Objects;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SCS_Logbook.View.Management.Usermanagement
{
    public partial class EditUserView : EditView<User>
    {
        public EditUserView()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            toEdit.Username = tb_username.Text;
            toEdit.Password = tb_password.Text;
            MySqlConnector.Instance.GetDbContext().SaveChanges();
            MySqlConnector.Instance.EndTransaction();
            Logbook.Instance.closeView(GetType());
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            MySqlConnector.Instance.RollbackTransaction();
            Logbook.Instance.closeView(GetType());
        }

        private void tb_username_TextChanged(object sender, EventArgs e)
        {
            if (!MySqlConnector.Instance.GetDbContext().Users.Where(user => user.Username.Equals(tb_username.Text)).Any() || tb_username.Text == toEdit.Username)
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

        public override void SetEdit(User toEdit)
        {
            base.SetEdit(toEdit);

            tb_id.Text = toEdit.Id.ToString();
            tb_username.Text = toEdit.Username;
            tb_password.Text = toEdit.Password;
        }
    }
}
