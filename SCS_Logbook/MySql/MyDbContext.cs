using System.Data.Common;
using System.Data.Entity;
using MySql.Data.Entity;
using SCS_Logbook.Objects;
using SCS_Logbook.Objects.Constants;

namespace SCS_Logbook.MySql
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class MyDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Company> Companies { get; set; }

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
