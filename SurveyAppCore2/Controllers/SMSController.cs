using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
//using Twilio;
//using Twilio.Rest.Api.V2010.Account;
//using Twilio.Types;
//using Twilio.TwiML;
//using Twilio.AspNet.Core;

namespace SurveyAppCore2.Controllers
{
    public class SMSController : Controller
    {

        public SMSController()
        {

        }

        public void SendSMS()
        {
            //var accountSID = "AC0205a0685f9ff66e2a7a417bda97c01b";
            //var authoKey = "a30e1342ae3471f529df9e698d14973a";

            //TwilioClient.Init(accountSID, authoKey);

            //var to = new PhoneNumber("+97470365521");


            //var ret = MessageResource.Create(to: to, from: new PhoneNumber("+14694164451"), body: "Test twilio for survey app!");

        }
    }
}