using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SurveyAppCore2.Models;
using SurveyAppCore2.DataProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SurveyAppCore2.Services;
using Microsoft.Extensions.Logging;

namespace SurveyAppCore2.Controllers
{
    [Authorize()]
    public class ReportsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;


        public ReportsController(
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

        public IActionResult GeneralSurveyReport()
        {

            return View();
        }
        public IActionResult FeedbackReport()
        {

            return View();
        }

        public IActionResult ManagerReport()
        {

            return View();
        }


        public IActionResult StaffReport()
        {

            return View();
        }

        public IActionResult CustomerReport()
        {

            return View();
        }

        public IActionResult OutletReport()
        {

            return View();
        }


        [Route("/api/GetCustomerReport")]
        public async Task<IActionResult> GetCustomerReport([FromBody]GeneralSurvey model)
        {
            var username = _userManager.GetUserName(User);
            UserDetailDataProvider userDetailDataProvider = new UserDetailDataProvider();

            var userDetails = await userDetailDataProvider.GetUserDetailByEmail(username);




            ReportDataProvider reportDataProvider = new ReportDataProvider();
            var survey = await reportDataProvider.GetCustomerReport(model, userDetails.SettingsId);
            //await surveyProvider.UpdateSurvey(survey);
            return Json(survey);
        }




        [Route("/api/GetGeneralSurveyReport")]
        public async Task<IActionResult> GetGeneralSurveyReport([FromBody]GeneralSurvey model)
        {
            var username = _userManager.GetUserName(User);
            UserDetailDataProvider userDetailDataProvider = new UserDetailDataProvider();

            var userDetails = await userDetailDataProvider.GetUserDetailByEmail(username);




            ReportDataProvider reportDataProvider = new ReportDataProvider();
            var survey = await reportDataProvider.GetGeneralSurveyReport(model, userDetails.SettingsId);
            //await surveyProvider.UpdateSurvey(survey);
            return Json(survey);
        }


        [Route("/api/GetFeedbackReport")]
        public async Task<IActionResult> GetFeedbackReport([FromBody]GeneralSurvey model)
        {
            var username = _userManager.GetUserName(User);
            UserDetailDataProvider userDetailDataProvider = new UserDetailDataProvider();

            var userDetails = await userDetailDataProvider.GetUserDetailByEmail(username);




            ReportDataProvider reportDataProvider = new ReportDataProvider();
            var survey = await reportDataProvider.GetFeedbackReport(model, userDetails.SettingsId);
            //await surveyProvider.UpdateSurvey(survey);
            return Json(survey);
        }

        
        [Route("/api/GetStaffReport")]
        public async Task<IActionResult> GetStaffReport([FromBody]GeneralSurvey model)
        {
            var username = _userManager.GetUserName(User);
            UserDetailDataProvider userDetailDataProvider = new UserDetailDataProvider();

            var userDetails = await userDetailDataProvider.GetUserDetailByEmail(username);

            ReportDataProvider reportDataProvider = new ReportDataProvider();
            var staffReport = await reportDataProvider.GetStaffReport(model, userDetails.SettingsId);
            //await surveyProvider.UpdateSurvey(survey);
            return Json(staffReport);
        }


        [Route("/api/GetManagerReport")]
        public async Task<IActionResult> GetManagerReport([FromBody]GeneralSurvey model)
        {
            var username = _userManager.GetUserName(User);
            UserDetailDataProvider userDetailDataProvider = new UserDetailDataProvider();

            var userDetails = await userDetailDataProvider.GetUserDetailByEmail(username);

            ReportDataProvider reportDataProvider = new ReportDataProvider();
            var managerReport = await reportDataProvider.GetManagerReport(model, userDetails.SettingsId);
            //await surveyProvider.UpdateSurvey(survey);
            return Json(managerReport);
        }

 
        [Route("/api/GetOutletReport")]
        public async Task<IActionResult> GetOutletReport([FromBody]GeneralSurvey model)
        {
            var username = _userManager.GetUserName(User);
            UserDetailDataProvider userDetailDataProvider = new UserDetailDataProvider();

            var userDetails = await userDetailDataProvider.GetUserDetailByEmail(username);

            ReportDataProvider reportDataProvider = new ReportDataProvider();
            var outletReport = await reportDataProvider.GetOutletReport(model, userDetails.SettingsId);
            //await surveyProvider.UpdateSurvey(survey);
            return Json(outletReport);
        }


    }
}