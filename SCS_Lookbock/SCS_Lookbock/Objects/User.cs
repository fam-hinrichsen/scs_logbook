using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCS_Lookbock.Objects
{
    [Table("user")]
    public class User
    {
        [Key]
        [Column("iduser")]
        public int Id { get;set; }
        [Column("username")]
        public string Username { get;set; }
        [Column("password")]
        public string Password { get;set;}
    }
}
