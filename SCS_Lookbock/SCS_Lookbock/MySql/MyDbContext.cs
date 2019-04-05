using System.Data.Common;
using System.Data.Entity;
using MySql.Data.Entity;
using SCS_Lookbock.Objects;

namespace SCS_Lookbock.MySql
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class MyDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Job> Jobs { get; set; }

        public MyDbContext():base("MyContext")
        {
        }

        // Constructor to use on a DbConnection that is already opened
        public MyDbContext(DbConnection existingConnection, bool contextOwnsConnection)
          : base(existingConnection, contextOwnsConnection)
        {
        }
    }
}
