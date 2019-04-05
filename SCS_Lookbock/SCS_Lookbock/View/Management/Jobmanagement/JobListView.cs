using SCS_Lookbock.Objects;
using System;
using System.Data.Entity;

namespace SCS_Lookbock.View.Management.Jobmanagement
{
    class JobListView : ListView<Job>
    {
        public JobListView(DbSet<Job> list, Type edit, Type add) : base(list, edit, add)
        {
            list.Include("Owner").Load();
            dg_Data.Columns["OwnerForeignKey"].Visible = false;
            Text = "Job";
        }
    }
}
