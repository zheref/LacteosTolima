using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LacteosTolima.App.Models
{
    public class ConsumptionReportHerd
    {
        public Int32 IdHerd { set; get; }

        public String NameHerd { set; get; }

        public String NameOpe { set; get; }

        public Int32 HowCows { set; get; }

        public Double SilageAmout { get; set; }

        public Double HayAmount { get; set; }
    }
}