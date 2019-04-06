using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCS_Logbook.Objects
{
    [Table("user")]
    public class User
    {
        [Key]
        [Column("iduser")]
        public int Id { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [InverseProperty("Owner")]
        public List<Job> Jobs { get; set; }

        public User()
        {
            Jobs = new List<Job>();
        }

        public override string ToString()
        {
            return Username;
        }
    }
}
