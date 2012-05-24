using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;


namespace LacteosTolima.App.Models
{
    [Table("CONSUMPTION")]
    public class Consumption
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_CON")]
        public Int32 Id { get; set; }

        [DisplayName("Consumption Silage")]
        [Required(ErrorMessage = "The Consumption of Silage is required")]
        [Range(1, 400, ErrorMessage = "The Consumption of Silage must to be between 1 and 400")]
        [Column("SILAGE_CON")]
        public Int16 SilageAmout { get; set; }

        [DisplayName("Consumption Hay")]
        [Required(ErrorMessage = "The Consumption of Hay is required")]
        [Range(1, 400, ErrorMessage = "The Consumption of Hay must to be between 1 and 400")]
        [Column("HAY_CON")]
        public Int16 HayAmount { get; set; }

        [DisplayName("Date")]
        [Required(ErrorMessage = "The Date is required")]
        [DataType(DataType.DateTime)]
        [Column("DATE_CON")]
        public DateTime Date { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("COWID_CON")]
        public Int32 CowId { get; set; } // FK of the Milker

        [ForeignKey("CowId")]
        [InverseProperty("Consumptions")]
        public virtual Cow Cow { get; set; }

    }
}