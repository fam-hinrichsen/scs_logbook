using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCS_Lookbock.Objects
{
    [Table("job")]
    public class Job
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("income")]
        public float Income { get; set; }
        [Column("distance")]
        public float Distance { get; set; }
        [Column("owner")]
        public int OwnerForeignKey { get; set; }
        [ForeignKey("OwnerForeignKey")]
        public virtual User Owner { get; set;}
    }
}
