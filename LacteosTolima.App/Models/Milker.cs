using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;


namespace LacteosTolima.App.Models
{
    [Table("MILKER")]
    public class Milker
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("MIL_ID")]
        public Int32 Id { get; set; }

        [DisplayName("Milker Name")]
        [Required(ErrorMessage = "The Milker Name is required")]
        [MaxLength(50, ErrorMessage = "Milker Name lenght can't be more than 50")]
        [MinLength(4, ErrorMessage = "Milker Name length can't be less than 4")]
        [Column("MIL_NAME")]
        public String Name { get; set; }

        [DisplayName("Milker Identification Number")]
        [Required(ErrorMessage = "The Identificacion is required")]
        [MaxLength(12, ErrorMessage = "Milker Name lenght can't be more than 50")]
        [MinLength(4, ErrorMessage = "Milker Name length can't be less than 4")]
        [Column("MIL_DOC")]        
        public String Doc { get; set; }

        [DisplayName("Milker Document Type")]
        [Required(ErrorMessage = "The Document Type is required")]
        [MaxLength(2, ErrorMessage = "Milker Document Type lenght can't be more than 2")]
        [MinLength(2, ErrorMessage = "Milker Document Type length can't be less than 2")]
        [Column("MIL_DOCTYPE")]
        public String DocType { get; set; }

        [DisplayName("Join Date")]
        [Required(ErrorMessage = "The Join Date is required")]
        [DataType(DataType.DateTime)]
        [Column("MIL_JOINDATE")]
        public DateTime JoinDate { get; set; }

        [DisplayName("Milker Age")]
        [Required(ErrorMessage = "The Milker Age is required")]
        [Range(0, 60, ErrorMessage = "The Age must to be between 0 and 60")]
        [Column("MIL_AGE")]
        public Byte Age { get; set; }

        [Column("MIL_STATE")]
        public String State { get; set; }

        public virtual List<Herd> Herds { get; set; }

    }
}