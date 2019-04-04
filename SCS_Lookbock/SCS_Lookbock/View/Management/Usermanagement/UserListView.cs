using SCS_Lookbock.Objects;
using System;
using System.Data.Entity;

namespace SCS_Lookbock.View.Management.Usermanagement
{
    public class UserListView : ListView<User>
    {
        public UserListView(DbSet<User> list, Type edit, Type add) : base(list, edit, add)
        {
            Text = "User";
        }
    }
}
