using SCS_Lookbock.Objects.Constants;
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

        [Column("citysourceid")]
        public int CitySourceForeignKey { get; set; }

        [Column("citydestinationid")]
        public int CityDestinationForeignKey { get; set; }

        [Column("companydestinationid")]
        public int CompanyDestinationForeignKey { get; set; }

        [Column("companysourceid")]
        public int CompanySourceForeignKey { get; set; }

        [Column("cargomass")]
        public float CargoMass { get; set; }

        [Column("cargoid")]
        public int CargoForeignKey { get; set; }

        [Column("active")]
        public bool IsActive { get; set; }

        [ForeignKey("OwnerForeignKey")]
        public virtual User Owner { get; set; }

        [ForeignKey("CityDestinationForeignKey")]
        public virtual City CityDestination { get; set; }

        [ForeignKey("CitySourceForeignKey")]
        public virtual City CitySource { get; set; }

        [ForeignKey("CompanyDestinationForeignKey")]
        public virtual Company CompanyDestination { get; set; }

        [ForeignKey("CompanySourceForeignKey")]
        public virtual Company CompanySource { get; set; }

        [ForeignKey("CargoForeignKey")]
        public virtual Cargo Cargo { get; set; }
    }
}
