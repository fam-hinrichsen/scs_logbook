using log4net;
using SCS_Lookbock.MySql;
using SCS_Lookbock.Objects;
using System;
using System.Data.Entity;
using System.Drawing;

namespace SCS_Lookbock.View.Management.Jobmanagement
{
    public partial class NewJobView : AddView<Job>
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            if (!float.TryParse(tb_income.Text, out float tmp))
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
            if (!float.TryParse(tb_distance.Text, out float tmp))
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
                if (float.TryParse(tb_income.Text, out float income) && float.TryParse(tb_distance.Text, out float distance))
                {
                    Job job = new Job
                    {
                        Name = tb_name.Text,
                        Income = income,
                        Distance = distance,
                        Owner = (User)cb_owner.Items[cb_owner.SelectedIndex]
                    };
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
                log.Error("Could not create a new job.", ex);
                MySqlConnector.Instance.RollbackTransaction();
            }
        }
    }
}
