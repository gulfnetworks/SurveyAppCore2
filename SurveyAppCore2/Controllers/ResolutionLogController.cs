using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SurveyAppCore2.DataProvider;
using SurveyAppCore2.Models;
using Microsoft.AspNetCore.Identity;
using SurveyAppCore2.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace SurveyAppCore2.Controllers
{
    [Authorize()]
    public class ResolutionLogController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

        public ResolutionLogController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<ResolutionLogController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }


        public IActionResult Index()
        {
            return View();
        }


        private DateTime ConvertToTimeZone(DateTime dateTime, string TimeZone)
        {
            var info = TimeZoneInfo.FindSystemTimeZoneById(TimeZone);
            return TimeZoneInfo.ConvertTime(dateTime, info);
        }



        public async Task<IActionResult> Create(string Id)
        {
            try
            {

         

        
                var username = _userManager.GetUserName(User);
                UserDetailDataProvider userDetailDataProvider = new UserDetailDataProvider();

                var userDetails = await userDetailDataProvider.GetUserDetailByEmail(username);

                var userDetailsComplete = await userDetailDataProvider.GetUserDetailCompleteById(userDetails.UserDetailId);



                SettingsDataProvider settingsDataProvider = new SettingsDataProvider();

                var settings = await settingsDataProvider.GetSettings(userDetails.SettingsId);



                ViewData["UserDetailComplete"] = userDetailsComplete.UserFirstName + " (" +
                                                    userDetailsComplete.JobTitleDesc + " @ " + userDetailsComplete.OutletName + ")";
                ViewData["SurveyId"] = Id;
                ViewData["UserDetailId"] = userDetailsComplete.UserDetailId;


                ResolutionLogDataProvider resolutionLogDataProvider = new ResolutionLogDataProvider();
                var lastId = await resolutionLogDataProvider.GetResolutionLogLastInsertedId();
                ViewData["LastInsertedId"] = (lastId + 1);


                DateTime dt = ConvertToTimeZone(DateTime.Now, settings.TimeZone);
                ViewData["CurrentDateTimeUTC"] = dt.ToString("yyyy-MM-dd HH:mm:ss");

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("~/Error");

            }
        }

        [Route("/api/AddResolutionLog")]
        public async Task<IActionResult> AddResolutionLog([FromBody]ResolutionLog resolutionLog)
        {
 
                ResolutionLogDataProvider resolutionLogDataProvider = new ResolutionLogDataProvider();

                var resolutionLogs = await resolutionLogDataProvider.AddResolutionLog(resolutionLog);
                var x = await resolutionLogDataProvider.UpdateSurveyStatusById(resolutionLog.SurveyId, resolutionLog.Status);

                
                return Json(resolutionLog.SurveyId);
            //return RedirectToAction(nameof(ResolutionLogController.Create), "ResolutionLog",new { SurveyId = resolutionLog.SurveyId });



        }

        [Route("/api/GetResolutionLogBySurveyId")]
        public async Task<IActionResult> GetResolutionLogBySurveyId([FromBody]string surveyId)
        {
            ResolutionLogDataProvider resolutionLogDataProvider = new ResolutionLogDataProvider();
            var resolutionLogs = await resolutionLogDataProvider.GetResolutionLogBySurveyId(int.Parse(surveyId));
            return Json(resolutionLogs);

        }

        [Route("/api/GetSurveyDataCompleteByIdForResolutionLog")]
        public async Task<IActionResult> GetSurveyDataCompleteById([FromBody]string surveyId)
        {
            SurveyDataProvider surveyDataProvider = new SurveyDataProvider();
            var surveyData = await surveyDataProvider.GetSurveyCompleteById(int.Parse(surveyId));
            return Json(surveyData);

        }

        
    }
}