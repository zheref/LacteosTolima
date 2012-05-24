using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;


namespace LacteosTolima.App.Models
{   [Table("COW")]
    public class Cow
    {
        [Required]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("COW_ID")]
        public Int32 Id { get; set; }

        [DisplayName("Cow Name")]
        [MaxLength(30,ErrorMessage="Cow Name lenght can't be more than 30")]
        [MinLength(3,ErrorMessage="Cow Name length can't be less than 3")]
        [Column("COW_NAME")]
        public String Name { get; set; }

        [DisplayName("Amount of Silage")]
        [Required(ErrorMessage = "The Amount of Silage is required")]
        [Range(1, 400, ErrorMessage = "The Amount of Silage must to be between 1 and 400")]
        [Column("COW_SILAGE")]
        public Int16 SilageAmout { get; set; }

        [DisplayName("Amount of Hay")]
        [Required(ErrorMessage = "The Amount of Hay is required")]
        [Range(1, 400, ErrorMessage = "The Amount of Hay must to be between 1 and 400")]
        [Column("COW_HAY")]
        public Int16 HayAmount { get; set; }

        [DisplayName("Cow Age")]
        [Required(ErrorMessage = "The Cow Age is required")]
        [Range(0, 40, ErrorMessage = "The Age must to be between 0 and 40")]
        [Column("COW_AGE")]
        public Byte Age { get; set; }

        [DisplayName("Commercial Assessment")]
        [Range(1, 100000000, ErrorMessage = "The Commercial Assessment must to be more than 0")]
        [Column("COW_COMMASS")]
        public Int64? CommercialAssessment { get; set; }

        [Column("COW_STATE")]
        [MaxLength(1)]
        public String State { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("COW_MOTHERID")]
        public Int32? MotherId { get; set; } // FK of the Cow Mother

        [ForeignKey("MotherId")]
        [Column("MOTHER_COW")]
        [InverseProperty("Children")]
        public virtual Cow Mother { get; set; }

        public List<Cow> Children { get; set; }

        [Range(1, 400, ErrorMessage = "The Herd doesn't exist")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HERDID_COW")]
        public Int32? HerdId { get; set; } // FK of the Herd

        [ForeignKey("HerdId")]
        [InverseProperty("Cows")]
        public virtual Herd Herd { set; get; }

        public List<Production> Productions { get; set; }   //lista de producciones de la vaca

        public List<Consumption> Consumptions { get; set; }  //lista de alimento de la vaca
    }
}