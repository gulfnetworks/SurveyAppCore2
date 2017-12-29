using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyAppCore2.Models
{
    public class Settings
    {
        public int SettingsId { get; set; }
        public string TimeZone { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyContact { get; set; }
        public string CompanyLogoUrl { get; set; }
        public string Country { get; set; }
        public string CompanyCode { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string ExpiryDate { get; set; }
    }
}
