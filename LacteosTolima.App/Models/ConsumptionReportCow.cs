using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LacteosTolima.App.Models
{
    public class ConsumptionReportCow
    {
        public Int32 IdCow { set; get; }

        public String NameCow { set; get; }

        public Double SilageAmout { get; set; }

        public Double HayAmount { get; set; }
    }
}