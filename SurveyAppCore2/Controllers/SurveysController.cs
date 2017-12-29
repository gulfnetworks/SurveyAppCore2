using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SurveyAppCore2.Models;
using Microsoft.AspNetCore.Authorization;
using SurveyAppCore2.DataProvider;
using Microsoft.AspNetCore.Identity;
using SurveyAppCore2.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace SurveyAppCore2.Controllers
{

    [AllowAnonymous]
    public class SurveysController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private HttpContext _currentContext;
        private readonly IHostingEnvironment _hostingEnvironment;

        public SurveysController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<ResolutionLogController> logger,
            IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _currentContext = httpContextAccessor.HttpContext;
            _hostingEnvironment = hostingEnvironment;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string Company)
        {
            SettingsDataProvider settingsDataProvider = new SettingsDataProvider();
            string CurrentCompany = String.Empty;
            if (User.Identity.IsAuthenticated)
            {
                var username = _userManager.GetUserName(User);
                UserDetailDataProvider userDetailDataProvider = new UserDetailDataProvider();

                var userDetails = await userDetailDataProvider.GetUserDetailByEmail(username);


               
                var settings = await settingsDataProvider.GetSettings(userDetails.SettingsId);

                if(Company != null)
                if(settings.CompanyCode != null)
                if(settings.CompanyCode.Trim().ToLower() != Company.Trim().ToLower())
                {
                    await _signInManager.SignOutAsync();

                        return View(settings);
                }


            }
    


            var settings2 = await settingsDataProvider.GetSettingsByCompanyCode(Company);

            if(settings2.CompanyName == null)
            {
                return RedirectToAction("PageNotFound", "Surveys");
            }
            ViewData["CompanyName"] = settings2.CompanyName;
            ViewData["CompanyCode"] = settings2.CompanyCode;
            return View();
        }

        //[Route("/api/GetUserDetailCompleteByManagerId2")]
        //public async Task<IActionResult> GetUserDetailCompleteByManagerId(string ManagerId)
        //{
        //    int managerId;
        //    bool result = Int32.TryParse(ManagerId, out managerId);
        //    if (result)
        //    {
        //        UserDetailDataProvider userDetailProvider = new UserDetailDataProvider();
        //        var test = await userDetailProvider.GetUserDetailCompleteByManagerId(managerId);
        //        return Json(test);
        //    }

        //    return Json("");
        //}


        [HttpGet]
        [Route("/api/GetAllStaffByOutletId")]
        public async Task<IActionResult> GetAllStaffByOutletId(string Company, int OutletId )
        {
            SettingsDataProvider settingsDataProvider = new SettingsDataProvider();

            var xxx = await settingsDataProvider.GetSettingsByCompanyCode(Company);

            UserDetailDataProvider userDetailProvider = new UserDetailDataProvider();
            var test = await userDetailProvider.GetAllStaffCompleteByOutletId(xxx.SettingsId, OutletId);
            return Json(test);
        }

        [HttpGet]
        [Route("/api/GetAllManager2")]
        public async Task<IActionResult> GetAllManager(string Company)
        {
            SettingsDataProvider settingsDataProvider = new SettingsDataProvider();

            var xxx = await settingsDataProvider.GetSettingsByCompanyCode(Company);

            UserDetailDataProvider userDetailProvider = new UserDetailDataProvider();
            var test = await userDetailProvider.GetManagers(xxx.SettingsId);
            return Json(test);
        }

        [HttpGet]
        [Route("/api/GetAllOutlet2")]
        public async Task<IActionResult> GetAllOutlet()
        {
            OutletDataProvider outletDataProvider = new OutletDataProvider();
            var test = await outletDataProvider.GetOutlets();
            return Json(test);
        }


        public IActionResult GeneralSurvey(string SurveyId,string Company, string Code)
        {
            ViewData["LastSurveyId"] = SurveyId;
            ViewData["Company"] = Company;
            ViewData["Code"] = Code;
            return View();
        }

        public IActionResult ThankYou(string Company, string Code)
        {
            ViewData["Code"] = Code;
            ViewData["Company"] = Company;
            return View();
        }
        public IActionResult PageNotFound()
        {
            return View("NotFound");
        }


        public IActionResult SurveyDetail()
        {
            return View();
        }

        public IActionResult CustomerDetail()
        {
            return View();
        }

        private void TempAddNewSurvey()
        {

        }


        [HttpPost]
        [Route("/Surveys/AddNewSurvey")]
        public async Task<IActionResult> AddNewSurvey([FromBody]Survey model)
        {

         
            SurveyDataProvider surveyProvider = new SurveyDataProvider();

            SettingsDataProvider settingsDataProvider = new SettingsDataProvider();

            var xxx = await settingsDataProvider.GetSettingsByCompanyCode(model.Action);
            

            Survey surveymodel = new Survey();
            surveymodel.CheckNo = model.CheckNo;

            surveymodel.Action = model.Action;
            surveymodel.Status = model.Status;
            surveymodel.Customer = model.Customer;
            surveymodel.MobileNo = model.MobileNo;
            surveymodel.Email = model.Email;
            surveymodel.CheckNo = model.CheckNo;
            surveymodel.TableNo = model.TableNo;
            surveymodel.ManagerId = model.ManagerId;
            surveymodel.StaffId = model.StaffId;
            surveymodel.OutletId = model.OutletId;
            var surveyId = await surveyProvider.AddSurvey(surveymodel, xxx.TimeZone);
            //return RedirectToAction(nameof(GeneralSurvey), new { SurveyId = surveyId, Company = xxx.CompanyName,Code=xxx.CompanyCode });
            //return StatusCode(200, "id:" + surveyId.ToString());


            string url = $"/Surveys/GeneralSurvey?SurveyId={surveyId.ToString()}&Company={xxx.CompanyName}&Code={ xxx.CompanyCode}";
            string completeUrl = CombineUrl(GetBaseUrl(), url);
            return Json(new { url = completeUrl });
        }



        public string CombineUrl(string baseUrl,string url)
        {
            Uri baseUri = new Uri(baseUrl);
            Uri myUri = new Uri(baseUri, url);
            return myUri.ToString();
        }


        public string GetBaseUrl()
        {
            var request = _currentContext.Request;

            var host = request.Host.ToUriComponent();

            var pathBase = request.PathBase.ToUriComponent();

            return $"{request.Scheme}://{host}{pathBase}";
        }


        [Route("/api/UpdateSurvey")]
        public async Task<IActionResult> UpdateSurvey([FromBody]Survey model)
        {

            SettingsDataProvider settingsDataProvider = new SettingsDataProvider();

            var xxx = await settingsDataProvider.GetSettingsByCompanyCode(model.Action);


            SurveyDataProvider surveyProvider = new SurveyDataProvider();
            var survey = await surveyProvider.GetSurveyById(model.SurveyId);
            survey.AmbienceComment = model.AmbienceComment;

            survey.QualityRate = model.QualityRate;
            survey.QualityComment = model.QualityComment;
            survey.ValueRate = model.ValueRate;
            survey.ValueComment = model.ValueComment;
            survey.ServiceRate = model.ServiceRate;
            survey.ServiceComment = model.ServiceComment;
            survey.AmbienceRate = model.AmbienceRate;
            survey.AmbienceComment = model.AmbienceComment;
            survey.RecommendRate = model.RecommendRate;
            survey.RecommendPoorArea = model.RecommendPoorArea;
            survey.RecommendImprovements = model.RecommendImprovements;
            survey.RecommendSuggestions = model.RecommendSuggestions;
            survey.Status= model.Status;
            await surveyProvider.UpdateSurvey(survey, xxx.TimeZone);
   
   
            return Json(xxx.SettingsId.ToString() + "|" + survey.Email);

        }

        public class TempThankyouClass
        {
            public int SettingsId { get; set; }
            public string Email { get; set; }
        }

        [Route("/api/SendThankyouEmail")]
        public async Task<IActionResult> SendThankyouEmail([FromBody] TempThankyouClass tmpClass)
        {

            // SEND THANK YOU EMAIL TO CUSTOMER



            EmailDataProvider edp = new EmailDataProvider();
            Email xx = await edp.GetThankEmail(tmpClass.SettingsId);

   
            string subject = xx.Subject;
            string content = xx.Content;
            string email = tmpClass.Email;
            string path = _hostingEnvironment.WebRootPath;

            SmtpOptions so = new SmtpOptions();
            so.Password = "Demo@dm1n";
            so.Port = 587;
            so.Server = "smtp.gmail.com";
            so.UseSsl = false;
            so.User = "gulfnetworks2017@gmail.com";
            so.RequiresAuthentication = true;

            Models.EmailSender es = new Models.EmailSender();


            await es.SendEmailAsync(so, email, "gulfnetworks2017@gmail.com", subject, "", content, path);
           // await es.SendEmailAsync(so, email, "gulfnetworks2017@gmail.com", subject, "", content, _hostingEnvironment.WebRootPath);


            return Json(new { Success = true });

        }

        [Route("/api/GetSurveyById")]
        public async Task<IActionResult> GetSurveyById([FromBody]string surveyId)
        {

            SurveyDataProvider surveyProvider = new SurveyDataProvider();
            

            var survey = await surveyProvider.GetSurveyById(int.Parse(surveyId));

            return Json(new { Success = true });

        }


    }
}