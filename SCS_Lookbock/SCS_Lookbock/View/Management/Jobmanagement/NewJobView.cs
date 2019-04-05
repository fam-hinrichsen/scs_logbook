using SCS_Lookbock.MySql;
using SCS_Lookbock.Objects;
using System;
using System.Data.Entity;
using System.Drawing;

namespace SCS_Lookbock.View.Management.Jobmanagement
{
    public partial class NewJobView : AddView<Job>
    {
        public NewJobView()
        {
            InitializeComponent();
            MySqlConnector.Instance.GetDbContext().Users.Load();
            cb_owner.DataSource = MySqlConnector.Instance.GetDbContext().Users.Local.ToBindingList();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            Logbook.Instance.closeView(GetType());
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

        private void btn_create_Click(object sender, EventArgs e)
        {
            try { 
                float distance;
                float income;
                if (float.TryParse(tb_income.Text, out income) && float.TryParse(tb_distance.Text, out distance))
                {
                    Job job = new Job();
                    job.Name = tb_name.Text;
                    job.Income = income;
                    job.Distance = distance;
                    job.Owner = (User)cb_owner.Items[cb_owner.SelectedIndex];
                    job.OwnerForeignKey = job.Owner.Id;

                    MySqlConnector.Instance.BeginTransaction();
                    MySqlConnector.Instance.GetDbContext().Jobs.Add(job);
                    MySqlConnector.Instance.GetDbContext().SaveChanges();
                    MySqlConnector.Instance.EndTransaction();

                    Logbook.Instance.closeView(GetType());
                } 
            }
            catch(Exception ex)
            {
                MySqlConnector.Instance.RollbackTransaction();
            }
        }
    }
}
