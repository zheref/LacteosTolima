using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LacteosTolima.App.Models
{
    public class CowPerHerdReport
    {
        public Int32 IdCow { get; set; }

        public String NameCow { get; set; }

        public Double SilageAmout { get; set; }

        public Double HayAmount { get; set; }

        public Double Quant { get; set; }
    }
}