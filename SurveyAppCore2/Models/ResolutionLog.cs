using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyAppCore2.Models
{
    public class ResolutionLog
    {
        public int ResolutionLogId { get; set; }
        public DateTime DateTime { get; set; }
        public int UpdaterId { get; set; }
        public string ResolutionDetails { get; set; }
        public int SurveyId { get; set; }
        public string Status { get; set; }
        public string UpdaterName { get; set; }
        public string OutletName { get; set; }
        public string OutletCountry { get; set; }
        public string UTCDate { get; set; }
        public string UTCTime { get; set; }
    }
}
