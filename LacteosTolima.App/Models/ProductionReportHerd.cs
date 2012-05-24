using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LacteosTolima.App.Models
{
    public class ProductionReportHerd
    {
        public Int32 IdHerd { set; get; }

        public String NameHerd { set; get; }

        public String NameOpe { set; get; }

        public Double Quant { get; set; }
    }
}