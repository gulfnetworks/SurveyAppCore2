using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyAppCore2.Models
{
    public class SurveyComplete : Survey
    {
        public string Manager { get; set; }
        public string Staff { get; set; }
        public string OutletName { get; set; }
        public string OutletCountry { get; set; }
        public int Complaint { get; set; }
        public int Feedback { get; set; }
        public int Compliment { get; set; }
        public string UTCDate { get; set; }
        public string UTCTime { get; set; }
    }
}
