using log4net;
using SCS_Logbook.MySql;
using System;
using System.Data.Entity;
using System.Windows.Forms;

namespace SCS_Logbook.View.Management
{
    public partial class ListView<T> : Form where T : class
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private bool closing;
        private Type add;
        private Type edit;

        public ListView(DbSet<T> list, Type edit, Type add)
        {
            InitializeComponent();

            this.add = add;
            this.edit = edit;
            list.Load();
            dg_Data.DataSource = list.Local.ToBindingList();
            dg_Data.Refresh();
        }

        public ListView()
        {
        }

        private void dg_Users_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            MySqlConnector.Instance.BeginTransaction();
            IEditView<T> editor = (IEditView<T>)Activator.CreateInstance(edit);
            editor.SetEdit((T)dg_Data.Rows[e.RowIndex].DataBoundItem);
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
            editor.SetEdit((T)dg_Data.Rows[dg_Data.SelectedCells[0].RowIndex].DataBoundItem);
            editor.SetParent(this);
            Logbook.Instance.AddView((Form)editor);
            Enabled = false;
            ((Form)editor).Show();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            T entity = (T)dg_Data.Rows[dg_Data.SelectedCells[0].RowIndex].DataBoundItem;

            DialogResult result = MessageBox.Show(
                "Do you really want to delete object: " + entity.ToString(),
                "Question",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes) {
                try
                {
                    MySqlConnector.Instance.BeginTransaction();
                    MySqlConnector.Instance.GetDbContext().Entry(entity).State = EntityState.Deleted;
                    MySqlConnector.Instance.GetDbContext().SaveChanges();
                    MySqlConnector.Instance.EndTransaction();
                }
                catch(Exception ex)
                {
                    log.Error("Could not delete object.", ex);
                    MySqlConnector.Instance.RollbackTransaction();
                }
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

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MySqlConnector.Instance.GetDbContext().RefreshAll();
            dg_Data.Refresh();
        }
    }
}
