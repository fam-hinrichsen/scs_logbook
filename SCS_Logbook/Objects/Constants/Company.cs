﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCS_Logbook.Objects.Constants
{
    [Table("company")]
    public class Company
    {
        [Key]
        [Column("idcompany")]
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
