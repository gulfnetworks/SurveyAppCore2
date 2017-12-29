using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyAppCore2.Models
{
    public class Subscription
    {
        public int SubscriptionId { get; set; }
        public string SubscriptionName { get; set; }
        public string SubscriptionType { get; set; }
        public double SubscriptionPrice { get; set; }
    }
}
