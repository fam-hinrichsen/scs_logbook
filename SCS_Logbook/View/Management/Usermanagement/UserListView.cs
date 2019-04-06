using SCS_Logbook.Objects;
using System;
using System.Data.Entity;

namespace SCS_Logbook.View.Management.Usermanagement
{
    public class UserListView : ListView<User>
    {
        public UserListView(DbSet<User> list, Type edit, Type add) : base(list, edit, add)
        {
            dg_Data.Columns["Password"].Visible = false;
            Text = "User";
        }
    }
}
