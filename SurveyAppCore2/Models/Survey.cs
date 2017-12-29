using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyAppCore2.Models
{
    public class Survey
    {
        public int SurveyId { get; set; }
        public int QualityRate { get; set; }
        public string QualityComment { get; set; }
        public int ValueRate { get; set; }
        public string ValueComment { get; set; }
        public int ServiceRate { get; set; }
        public string ServiceComment { get; set; }
        public int AmbienceRate { get; set; }
        public string AmbienceComment { get; set; }
        public int RecommendRate { get; set; }
        public string RecommendPoorArea { get; set; }
        public string RecommendImprovements { get; set; }
        public string RecommendSuggestions { get; set; }
        public int LastVisit { get; set; }
        public string LastVisitComment { get; set; }
        public string Action { get; set; }
        public string Status { get; set; }
        public DateTime DateTime { get; set; }
        public string Customer { get; set; }
        public string MobileNo { get; set; }

        public string Email { get; set; }

        public string CheckNo { get; set; }

        public string TableNo { get; set; }

        public int ManagerId { get; set; }

        public int StaffId { get; set; }
        public int OutletId { get; set; }
    }
}
