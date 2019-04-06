using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCS_Lookbock.Objects.Constants
{
    [Table("city")]
    public class City
    {
        [Key]
        [Column("idcity")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("nameloc")]
        public string NameLocal { get; set; }

        public override string ToString()
        {
            return NameLocal;
        }
    }
}
