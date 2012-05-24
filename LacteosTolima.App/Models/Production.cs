using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace LacteosTolima.App.Models
{
    public enum Shift {MORNING,AFTERNOON}
    
    [Table("PRODUCTION")]
    public class Production
    {
        [Required]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("PROD_ID")]
        public Int32 Id { get; set; }

        [DisplayName("Quantity (Quarter Milk)")]
        [Required(ErrorMessage="The Quantity is required")]
        [Range(1,200,ErrorMessage="The Quantity must to be between 1 and 200")]
        [Column("PROD_QUANT")]
        public Double Quant { get; set; }

        [DisplayName("Date")]
        [Required(ErrorMessage="The Date is required")]
        [DataType(DataType.DateTime)]
        //[Range(typeof(DateTime),"01/01/1990",DateTime.Today.ToShortDateString())]
        [Column("PROD_DATE")]
        public DateTime Date { get; set; }

        [DisplayName("Shift")]
        [Required(ErrorMessage="The Shift is required")]
        [MaxLength(1, ErrorMessage = "Shift Code can't be more than 1")]
        [MinLength(1, ErrorMessage = "Shift Code can't be more than 1")]
        [Column("PROD_SHIFT")]
        public string Shift { get; set; }

        [DisplayName("Cow")]
        [Required(ErrorMessage="The Cow identification is required")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("PROD_COWID")]
        public Int32 CowId { get; set; } // FK of the Cow

        [ForeignKey("CowId")]
        [InverseProperty("Productions")]
        public virtual Cow Cow { get; set; }

    }
}