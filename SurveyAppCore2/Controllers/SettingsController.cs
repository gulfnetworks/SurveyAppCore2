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
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace SurveyAppCore2.Controllers
{
    [Authorize()]
    public class SettingsController : Controller
    {


        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _hostingEnvironment;

        public SettingsController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<ResolutionLogController> logger,
            IHostingEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Company()
        {
            return View();
        }

        public async Task<IActionResult> Time()
        {
            var username = _userManager.GetUserName(User);
            UserDetailDataProvider userDetailDataProvider = new UserDetailDataProvider();

            var userDetails = await userDetailDataProvider.GetUserDetailByEmail(username);

            SettingsDataProvider settingsDataProvider = new SettingsDataProvider();
            var settings = await settingsDataProvider.GetSettings(userDetails.SettingsId);

            ViewData["TimeZone"] = settings.TimeZone;

            return View();
        }


        [Route("/api/GetSettingsById")]
        public async Task<IActionResult> GetSettingsById([FromBody]string Id)
        {
            SettingsDataProvider settingsDataProvider = new SettingsDataProvider();
            var settings = await settingsDataProvider.GetSettings(int.Parse(Id));
            return Json(settings);
        }

        [Route("/api/GetCompanySettings")]
        public async Task<IActionResult> GetCompanySettings()
        {
            var username = _userManager.GetUserName(User);
            UserDetailDataProvider userDetailDataProvider = new UserDetailDataProvider();

            var userDetails = await userDetailDataProvider.GetUserDetailByEmail(username);


            SettingsDataProvider settingsDataProvider = new SettingsDataProvider();
            var settings = await settingsDataProvider.GetSettings(userDetails.SettingsId);

            return Json(settings);
        }

        [HttpPost]
        [Route("/api/UpdateLogo")]
        public IActionResult UploadFilesAjax()
        {
            Guid g = Guid.NewGuid();

            string filename2 = "";

            long size = 0;
            var files = Request.Form.Files;
            foreach (var file in files)
            {
                 var filename = ContentDispositionHeaderValue
                                .Parse(file.ContentDisposition)
                                .FileName
                                .Trim('"');
                filename2 = g + "." + filename.Split('.')[1];
            

                string webRootPath = _hostingEnvironment.WebRootPath;
                filename = webRootPath+ @"\images\logo"  + $@"\{filename2}";

                size += file.Length;
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            }
            //string message = @"{" + files.Count.ToString() + "} file(s) / {" + size.ToString() + "} bytes uploaded successfully!";
            return Json(@"/images/logo/" + filename2);
        }


        [Route("/api/UpdateCompanySettings")]
        public async Task<IActionResult> UpdateCompanySettings([FromBody]Settings model)
        {

            var username = _userManager.GetUserName(User);
            UserDetailDataProvider userDetailDataProvider = new UserDetailDataProvider();

            var userDetails = await userDetailDataProvider.GetUserDetailByEmail(username);

            SettingsDataProvider settingsDataProvider = new SettingsDataProvider();

            var settings = await settingsDataProvider.GetSettings(userDetails.SettingsId);
            settings.CompanyContact = model.CompanyContact;
            settings.CompanyAddress = model.CompanyAddress;

            if (model.CompanyLogoUrl != null && model.CompanyLogoUrl != "" && model.CompanyLogoUrl != "/images/logo/")
            {
                settings.CompanyLogoUrl = model.CompanyLogoUrl;
            }
            else
            {
                settings.CompanyLogoUrl = "";
            }

            settings.CompanyName = model.CompanyName;
            settings.Country = model.Country;
            settings.CompanyCode = model.CompanyCode;

            settings.EndTime = model.EndTime;
            settings.StartTime = model.StartTime;

            var res = await settingsDataProvider.UpdateSettings(settings);
            return Json(res);
        }

        [Route("/api/UpdateSettingsById")]
        public async Task<IActionResult> UpdateSettingsById([FromBody]string TimeZone)
        {

            var username = _userManager.GetUserName(User);
            UserDetailDataProvider userDetailDataProvider = new UserDetailDataProvider();

            var userDetails = await userDetailDataProvider.GetUserDetailByEmail(username);

            SettingsDataProvider settingsDataProvider = new SettingsDataProvider();

            var settings = await settingsDataProvider.GetSettings(userDetails.SettingsId);
            settings.TimeZone = TimeZone;

            var res = await settingsDataProvider.UpdateSettings(settings);
            return Json(res);
        }




        [Route("/api/AddCompanySettings")]
        public async Task<IActionResult> AddCompanySettings([FromBody]Settings model)
        {

            var username = _userManager.GetUserName(User);
            UserDetailDataProvider userDetailDataProvider = new UserDetailDataProvider();

            var userDetails = await userDetailDataProvider.GetUserDetailByEmail(username);

            SettingsDataProvider settingsDataProvider = new SettingsDataProvider();

            var settings = await settingsDataProvider.GetSettings(userDetails.SettingsId);
            settings.CompanyContact = model.CompanyContact;
            settings.CompanyAddress = model.CompanyAddress;

            if (model.CompanyLogoUrl != null && model.CompanyLogoUrl != "" && model.CompanyLogoUrl != "/images/logo/")
            {
                settings.CompanyLogoUrl = model.CompanyLogoUrl;
            }
            else
            {
                settings.CompanyLogoUrl = "";
            }

            settings.CompanyName = model.CompanyName;
            settings.Country = model.Country;
            settings.CompanyCode = model.CompanyCode;

            settings.EndTime = model.EndTime;
            settings.StartTime = model.StartTime;
            settings.ExpiryDate = model.ExpiryDate;

            var res = await settingsDataProvider.AddSettings(settings);
            return Json(res);
        }



        [Route("/api/GetAllCompany")]
        public async Task<IActionResult> GetAllCompany()
        {
            SettingsDataProvider settingsDataProvider = new SettingsDataProvider();
            var settings = await settingsDataProvider.GetAllCompany();
            return Json(settings);
        }


        [Route("/api/GetCompanySettingsById")]
        public async Task<IActionResult> GetCompanySettingsById(string Id)
        {
            if (Id != null)
            if (Id.Trim() != "")
            {
                int settingsId = int.Parse(Id);
                SettingsDataProvider settingsDataProvider = new SettingsDataProvider();
                var settings = await settingsDataProvider.GetSettings(settingsId);

                return Json(settings);
            }

            return Json("");
        }
   
    }
}