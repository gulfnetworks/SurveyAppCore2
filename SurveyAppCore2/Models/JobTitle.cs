using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyAppCore2.Models
{
    public class JobTitle
    {
        public int JobTitleId { get; set; }
        public string JobTitleDesc { get; set; }
        public int UserTypeId { get; set; }
    }
}
