using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyAppCore2.Models
{
    public class GeneralSurveyViewModel
    {
        public string Country { get; set; }
        public int OutletId { get; set; }
        public string DateStart { get; set; }
        public string DateEnd { get; set; }
        public string Segment { get; set; }
        public string Type { get; set; }
        public string Statuses { get; set; }
        public string KPIS { get; set; }

    }
}
