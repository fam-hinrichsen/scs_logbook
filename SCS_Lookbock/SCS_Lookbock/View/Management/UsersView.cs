using SCS_Lookbock.MySql;
using SCS_Lookbock.Objects;
using System;
using System.Data.Entity;
using System.Windows.Forms;

namespace SCS_Lookbock.View.Management
{
    public partial class UsersView : Form
    {
        private bool closing;
        public UsersView()
        {
            InitializeComponent(); 
            
            MySqlConnector.Instance.GetDbContext().Users.Load();
            dg_Users.DataSource = MySqlConnector.Instance.GetDbContext().Users.Local.ToBindingList();
            dg_Users.Columns["Password"].Visible = false;
            //dg_Users.Columns["Jobs"].Visible = false;
            dg_Users.Refresh();
        }
        
        private void dg_Users_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            MySqlConnector.Instance.BeginTransaction();
            EditUserView editUser = new EditUserView((User)dg_Users.Rows[e.RowIndex].DataBoundItem, this);
            Logbook.Instance.AddView(editUser);
            Enabled = false;
            editUser.Show();
        }

        private void UsersView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (closing)
            {
                return;
            }

            closing = true;
            e.Cancel = true;
            Logbook.Instance.closeView(GetType());
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MySqlConnector.Instance.BeginTransaction();
            EditUserView editUser = new EditUserView((User)dg_Users.Rows[dg_Users.SelectedCells[0].RowIndex].DataBoundItem, this);
            Logbook.Instance.AddView(editUser);
            Enabled = false;
            editUser.Show();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MySqlConnector.Instance.BeginTransaction();
            User user = (User)dg_Users.Rows[dg_Users.SelectedCells[0].RowIndex].DataBoundItem;

            DialogResult result = MessageBox.Show(
                "Do you really want to delete user: " + user.Username,
                "Question",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if(result == DialogResult.Yes) {
                MySqlConnector.Instance.GetDbContext().Users.Remove(user);
                MySqlConnector.Instance.GetDbContext().SaveChanges();
                MySqlConnector.Instance.EndTransaction();
            }
            else
            {
                MySqlConnector.Instance.RollbackTransaction();
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewUserView view = new NewUserView(this);
            Logbook.Instance.AddView(view); 
            Enabled = false;
            view.Show();
        }
    }
}
