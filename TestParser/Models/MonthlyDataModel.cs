using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestParser.Models
{
    public class MonthlyDataModel
    {
        public int Year { get; set; }
        public string Month { get; set; }
        public int LatinAmerica { get; set; }
        public int Europe { get; set; }
        public int Africa { get; set; }
        public int MiddleEast { get; set; }
        public int AsiaPacific { get; set; }
        public int TotalInternational { get; set; }
        public int Canada { get; set; }
        public int US { get; set; }
        public int TotalWorld { get; set; }
    }
}
