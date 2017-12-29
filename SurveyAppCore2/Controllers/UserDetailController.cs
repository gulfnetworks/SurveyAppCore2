using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SurveyAppCore2.DataProvider;
using SurveyAppCore2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SurveyAppCore2.Services;
using Microsoft.Extensions.Logging;

namespace SurveyAppCore2.Controllers
{
    [Authorize()]
    public class UserDetailController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;


        public UserDetailController(
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




        /* ADD USER DETAIL REGION */
        [HttpGet]
        [Route("/api/GetAllJobTitle")]
        public async Task<IActionResult> GetAllJobTitle()
        {
            JobTitleDataProvider jobTitleDataProvider = new JobTitleDataProvider();
            var test = await jobTitleDataProvider.GetJobTitles();
            return Json(test);
        }


        [HttpGet]
        [Route("/api/GetAllOutlet")]
        public async Task<IActionResult> GetAllOutlet()
        {
            OutletDataProvider outletDataProvider = new OutletDataProvider();
            var test = await outletDataProvider.GetOutlets();
            return Json(test);
        }

        [HttpGet]
        [Route("/api/GetAllManager")]
        public async Task<IActionResult> GetAllManager()
        {
            var username = _userManager.GetUserName(User);
            UserDetailDataProvider userDetailDataProvider = new UserDetailDataProvider();

            var userDetails = await userDetailDataProvider.GetUserDetailByEmail(username);


            UserDetailDataProvider userDetailProvider = new UserDetailDataProvider();
            var test = await userDetailProvider.GetManagers(userDetails.SettingsId);
            return Json(test);
        }


        [HttpGet]
        [Route("/api/GetAllSubscription")]
        public async Task<IActionResult> GetAllSubscription()
        {
            SubscriptionDataProvider subscriptionProvider = new SubscriptionDataProvider();
            var test = await subscriptionProvider.GetSubscriptions();
            return Json(test);
        }
        


          
        [Route("/api/GetUserDetailComplete")]
        public async Task<IActionResult> GetUserDetailComplete()
        {
            var username = _userManager.GetUserName(User);
            UserDetailDataProvider userDetailDataProvider = new UserDetailDataProvider();

            var userDetails = await userDetailDataProvider.GetUserDetailByEmail(username);

            UserDetailDataProvider userDetailProvider = new UserDetailDataProvider();
            var test = await userDetailProvider.GetUserDetailComplete(userDetails.SettingsId);
            return Json(test);
        }


        [Route("/api/GetUserDetailCompleteById")]
        public async Task<IActionResult> GetUserDetailCompleteById(string id)
        {
            int userDetailId;
            bool result = Int32.TryParse(id, out userDetailId);
            if (result)
            {
                UserDetailDataProvider userDetailProvider = new UserDetailDataProvider();
                var test = await userDetailProvider.GetUserDetailCompleteById(userDetailId);
                return Json(test);
            }

            return Json("");
        }

        [Route("/api/GetUserDetailCompleteByManagerId")]
        public async Task<IActionResult> GetUserDetailCompleteByManagerId(string ManagerId)
        {
            int managerId;
            bool result = Int32.TryParse(ManagerId, out managerId);
            if (result)
            {
                UserDetailDataProvider userDetailProvider = new UserDetailDataProvider();
                var test = await userDetailProvider.GetUserDetailCompleteByManagerId(managerId);
                return Json(test);
            }

            return Json("");
        }
        /********************************* END ************************************************/

    }
}