using log4net;
using SCS_Logbook.MySql;
using SCS_Logbook.Objects;
using System;
using System.Data.Entity;
using System.Drawing;

namespace SCS_Logbook.View.Management.Jobmanagement
{
    public partial class EditJobView : EditView<Job>
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public EditJobView()
        {
            InitializeComponent();
            MySqlConnector.Instance.GetDbContext().Users.Load();
            cb_owner.DataSource = MySqlConnector.Instance.GetDbContext().Users.Local.ToBindingList();
        }

        public override void SetEdit(Job toEdit)
        {
            base.SetEdit(toEdit);
            
            tb_id.Text = toEdit.Id.ToString();
            tb_name.Text = toEdit.Name;
            tb_income.Text = toEdit.Income.ToString();
            tb_distance.Text = toEdit.Distance.ToString();
            cb_owner.SelectedItem = toEdit.Owner;
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            Logbook.Instance.closeView(GetType());
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                float distance;
                float income;
                if (float.TryParse(tb_income.Text, out income) && float.TryParse(tb_distance.Text, out distance))
                {
                    MySqlConnector.Instance.BeginTransaction();
                    toEdit.Name = tb_name.Text;
                    toEdit.Income = income;
                    toEdit.Distance = distance;
                    toEdit.Owner = (User)cb_owner.Items[cb_owner.SelectedIndex];
                    toEdit.OwnerForeignKey = toEdit.Owner.Id;

                    MySqlConnector.Instance.GetDbContext().SaveChanges();
                    MySqlConnector.Instance.EndTransaction();

                    Logbook.Instance.closeView(GetType());
                }
            }
            catch (Exception ex)
            {
                log.Error("Could not create a new job.", ex);
                MySqlConnector.Instance.RollbackTransaction();
            }
        }

        private void tb_income_TextChanged(object sender, EventArgs e)
        {
            float tmp;
            if (!float.TryParse(tb_income.Text, out tmp))
            {
                tb_income.BackColor = Color.Red;
            }
            else
            {
                tb_income.BackColor = Color.LightGreen;
            }
        }

        private void tb_distance_TextChanged(object sender, EventArgs e)
        {
            float tmp;
            if (!float.TryParse(tb_distance.Text, out tmp))
            {
                tb_distance.BackColor = Color.Red;
            }
            else
            {
                tb_distance.BackColor = Color.LightGreen;
            }
        }
    }
}
