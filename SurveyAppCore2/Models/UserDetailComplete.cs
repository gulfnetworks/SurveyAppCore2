using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyAppCore2.Models
{
    public class UserDetailComplete
    {
        public int UserDetailId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string MobileNo { get; set; }
        public string UserEmail { get; set; }
        public int JobTitleId { get; set; }


        // USERDETAIL ID
        public int ManagerId { get; set; }
        public int OutletId { get; set; }
        public string ExtraOutletId { get; set; }
        public int SubscriptionId { get; set; }
        public int Active { get; set; }

        public string JobTitleDesc { get; set; }
        public int UserTypeId { get; set; }

        public string OutletName { get; set; }
        public string OutletAddress { get; set; }
        public string OutletCountry { get; set; }


        public string SubscriptionName { get; set; }
        public string SubscriptionType { get; set; }
        public double SubscriptionPrice { get; set; }
    }
}
