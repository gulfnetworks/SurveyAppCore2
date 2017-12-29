using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyAppCore2.Models
{
    public class OutletReport
    {
        public float Quality { get; set; }
        public float Value { get; set; }
        public float Service { get; set; }
        public float Ambience { get; set; }
        public float Recommend { get; set; }
        public float Overall { get; set; }
        public float NetPromoterScore { get; set; }
        public int SurveyCount { get; set; }
        public int DaysOnDuty { get; set; }
        public float DaysOutletMetQuota { get; set; }
        public int Trigger { get; set; }
        public string Staff { get; set; }
        public string Manager { get; set; }
        public string Outlet { get; set; }
        public string Country { get; set; }
        public int OutletDailyQuota { get; set; }
        public float LVMonths { get; set; }
        public float LVNever { get; set; }
        public float LV1 { get; set; }
        public float LV2 { get; set; }
        public float LV3 { get; set; }
        public float TriggerPercent { get; set; }
        public float UnresolveTrigger { get; set; }
        public float ResolveTriggerPercent { get; set; }
        public string Status { get; set; }


        public OutletReport()
        {
            this.Quality = 0;
            this.Value = 0;
            this.Service = 0;
            this.Ambience = 0;
            this.Recommend = 0;
            this.Overall = 0;
            this.NetPromoterScore = 0;
            this.SurveyCount = 0;
            this.DaysOnDuty = 0;
            this.DaysOutletMetQuota = 0;
            this.Trigger = 0;
            this.Staff = "";
            this.Manager = "";
            this.Outlet = "";
            this.Country = "";
            this.OutletDailyQuota = 0;


            this.LVMonths = 0;
            this.LVNever = 0;
            this.LV1 = 0;
            this.LV2 = 0;
            this.LV3 = 0;

            this.TriggerPercent = 0;
            this.UnresolveTrigger = 0;
            this.ResolveTriggerPercent = 0;
            this.Status = "";
        }

    }
}
