using SCS_Logbook.Objects;
using System;
using System.Data.Entity;

namespace SCS_Logbook.View.Management.Jobmanagement
{
    class JobListView : ListView<Job>
    {
        public JobListView(DbSet<Job> list, Type edit, Type add) : base(list, edit, add)
        {
            list.Include("Owner").Load();
            dg_Data.Columns["OwnerForeignKey"].Visible = false;
            dg_Data.Columns["CityDestinationForeignKey"].Visible = false;
            dg_Data.Columns["CitySourceForeignKey"].Visible = false;
            dg_Data.Columns["CompanyDestinationForeignKey"].Visible = false;
            dg_Data.Columns["CompanySourceForeignKey"].Visible = false;
            dg_Data.Columns["CargoForeignKey"].Visible = false;

            Text = "Job";
        }
    }
}
