using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SurveyAppCore2.Models;
using SurveyAppCore2.DataProvider;
using Microsoft.AspNetCore.Identity;
using SurveyAppCore2.Services;
using Microsoft.Extensions.Logging;

namespace SurveyAppCore2.Controllers
{

    public class LayoutController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

        public LayoutController(
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

        public async Task<IActionResult> CompanyName()
        {
            var username = _userManager.GetUserName(User);
            UserDetailDataProvider userDetailDataProvider = new UserDetailDataProvider();

            var userDetails = await userDetailDataProvider.GetUserDetailByEmail(username);


            SettingsDataProvider settingsDataProvider = new SettingsDataProvider();
            var settings = await settingsDataProvider.GetSettings(userDetails.SettingsId);
            return PartialView(settings);
        }
    }
}