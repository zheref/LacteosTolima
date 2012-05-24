using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace LacteosTolima.App.Models
{
    [Table("HERD")]
    public class Herd
    {
        [Column("HERD_ID")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }

        [DisplayName("Herd Name")]
        [MaxLength(30, ErrorMessage = "Herd Name lenght can't be more than 30")]
        [MinLength(3, ErrorMessage = "Herd Name length can't be less than 3")]
        [Column("HERD_NAME")]
        public String Name { get; set; }

        [DisplayName("Join Date")]
        [Required(ErrorMessage = "The Join Date is required")]
        [DataType(DataType.DateTime)]
        [Column("HERD_JOINDATE")]
        public DateTime JoinDate { get; set; }

        [Column("HERD_STATE")]
        public String State { get; set; }

        public List<Cow> Cows { get; set; }   //lista de vacas

        [DisplayName("Milker")]
        [Required(ErrorMessage = "The Milker is required")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HERD_MILID")]
        public Int32 MilkerId { get; set; } // FK of the Milker
               
        [ForeignKey("MilkerId")]
        [InverseProperty("Herds")]
        public virtual Milker Milker { get; set; }

    }
}