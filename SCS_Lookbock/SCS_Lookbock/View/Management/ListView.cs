using SCS_Lookbock.MySql;
using SCS_Lookbock.Objects;
using System;
using System.Data.Entity;
using System.Windows.Forms;

namespace SCS_Lookbock.View.Management
{
    public partial class ListView<T> : Form where T : class
    {
        private bool closing;
        private Type add;
        private Type edit;

        public ListView(DbSet<T> list, Type edit, Type add)
        {
            InitializeComponent();

            this.add = add;
            this.edit = edit;
            list.Load();
            dg_Users.DataSource = list.Local.ToBindingList();
            //dg_Users.Columns["Password"].Visible = false;
            //dg_Users.Columns["Jobs"].Visible = false;
            dg_Users.Refresh();
        }

        public ListView()
        {
        }

        private void dg_Users_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            MySqlConnector.Instance.BeginTransaction();
            IEditView<T> editor = (IEditView<T>)Activator.CreateInstance(edit);
            editor.SetEdit((T)dg_Users.Rows[e.RowIndex].DataBoundItem);
            editor.SetParent(this);
            Logbook.Instance.AddView((Form)editor);
            Enabled = false;
            ((Form)editor).Show();
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
            IEditView<T> editor = (IEditView<T>)Activator.CreateInstance(edit);
            editor.SetEdit((T)dg_Users.Rows[dg_Users.SelectedCells[0].RowIndex].DataBoundItem);
            editor.SetParent(this);
            Logbook.Instance.AddView((Form)editor);
            Enabled = false;
            ((Form)editor).Show();
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
            IAddView<T> adder = (IAddView<T>)Activator.CreateInstance(add);
            adder.SetParent(this);
            Logbook.Instance.AddView((Form)adder);
            Enabled = false;
            ((Form)adder).Show();
        }
    }
}
