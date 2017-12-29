using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MailKit;
using MimeKit;
using MailKit.Net.Smtp;
using SurveyAppCore2.Models;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Net.Http.Headers;
using SurveyAppCore2.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using SurveyAppCore2.DataProvider;
using System.Web;

namespace SurveyAppCore2.Controllers
{
    public class MailboxController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IHostingEnvironment _hostingEnvironment;

        public MailboxController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            IHostingEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _hostingEnvironment = hostingEnvironment;
        }



        public IActionResult Inbox()
        {
            return View();
        }

        public IActionResult EmailView()
        {
            return View();
        }

        public async  Task<IActionResult> ComposeEmail(string Id)
        {
            //SMSController _xxx = new SMSController();
            //_xxx.SendSMS();
            if(Id == null || Id == "")
            {
                ViewData["Email"] = "";
                return View();
            }

            SurveyDataProvider sdp = new SurveyDataProvider();
            var survey = await sdp.GetSurveyById(int.Parse(Id));


        
            ViewData["Email"] = survey.Email;
            return View();
        }


        [HttpPost]
        [Route("/api/UploadImage")]
        public IActionResult UploadImage()
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
                filename = webRootPath + @"\images\Email" + $@"\{filename2}";

                size += file.Length;
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            }
            //string message = @"{" + files.Count.ToString() + "} file(s) / {" + size.ToString() + "} bytes uploaded successfully!";
            return Json(@"/images/Email/" + filename2);
        }

        [Route("/api/SendEmail")]
        public async Task<IActionResult> SendEmail([FromBody]Email model)
        {
 


        
            //var message = new MimeMessage();
            //message.From.Add(new MailboxAddress("Test", "gulfnetworks2017@gmail.com"));
            //message.To.Add(new MailboxAddress("zaldy", "zaldy.piraman2015@gmail.com"));
            //message.Subject = "test email asp.net core";
            ////message.Body = new TextPart("plain")
            ////{
            ////    Text = "hello world mail"
            ////};

            //message.Body = new TextPart("html") { Text = "<b>Test Message</b>" };

            //using (var client = new SmtpClient())
            //{

            //    client.Connect("smtp.gmail.com", 587, false);
            //    client.Authenticate("gulfnetworks2017@gmail.com", "Demo@dm1n");
            //    client.Send(message);
            //    client.Disconnect(true);
            //}
            SmtpOptions so = new SmtpOptions();
            so.Password = "Demo@dm1n";
            so.Port = 587;
            so.Server = "smtp.gmail.com";
            so.UseSsl = false;
            so.User = "gulfnetworks2017@gmail.com";
            so.RequiresAuthentication = true;

            Models.EmailSender es = new Models.EmailSender();
            if (model.To.IndexOf(",") == -1)
            {
            

                await es.SendEmailAsync(so, model.To, "gulfnetworks2017@gmail.com", model.Subject, "", model.Content, _hostingEnvironment.WebRootPath);

            }
            else
            {
                await es.SendMultipleEmailAsync(so, model.To, "gulfnetworks2017@gmail.com", model.Subject, "", _hostingEnvironment.WebRootPath, model.Content);

            }


            return Json("ok");
        }



        [Route("/api/UpdateThankyouEmail")]
        public async Task<IActionResult> UpdateThankyouEmail([FromBody]Email model)
        {

            EmailDataProvider edp = new EmailDataProvider();

            Email tmp = await edp.GetEmailById(model.EmailId, model.SettingsId);
            tmp.Content = model.Content;
            tmp.Subject = model.Subject;
            tmp.From = model.From;


            await edp.UpdateEmail(tmp);


            return Json("ok");
        }

        public IActionResult EmailTemplates()
        {
            return View();
        }

        public IActionResult BasicActionEmail()
        {
            return View();
        }

        public IActionResult AlertEmail()
        {
            return View();
        }

        public IActionResult BillingEmail()
        {
            return View();
        }

        public async Task<IActionResult> ThankYouEmail()
        {
            var username = _userManager.GetUserName(User);
            UserDetailDataProvider userDetailDataProvider = new UserDetailDataProvider();

            var userDetails = await userDetailDataProvider.GetUserDetailByEmail(username);

            SettingsDataProvider settingsDataProvider = new SettingsDataProvider();
            var settings = await settingsDataProvider.GetSettings(userDetails.SettingsId);

            EmailDataProvider edp = new EmailDataProvider();
            Email xx =  await edp.GetThankEmail(userDetails.SettingsId);

            if(xx != null)
            {
                ViewData["Content"] = xx.Content;
                ViewData["EmailId"] = xx.EmailId;
                ViewData["Subject"] = xx.Subject;
                ViewData["From"] = xx.From;
                ViewData["SettingsId"] = xx.SettingsId;
            }
            else
            {

           

                Email email = new Email();
                email.IsRead = false;
                email.From = "gulfnetworks2017@gmail.com";
                email.To = "";
                email.SettingsId = userDetails.SettingsId;
                email.Subject = settings.CompanyName + " Survey";
                email.Content = "<div>" +
                                "<p></p>" +
                                "<p>Thank you for taking the time to complete this survey.  We truly value the information you have provided.  Your responses will contribute to our analyses of the texts and suggest new lines of approach to the corpus data. </p>" +
                                "<p>You can find the latest updates on the project here at this blog and you can also follow us on twitter by clicking on the link to the right.</p>"+
                                "<p>If you have any comments on the survey or the project, please leave a comment below.</p>"+
                                "<p><br/>Many thanks,</p>" +
                                "<p><br/><em>" + settings.CompanyName + " Team </em></p>" +
                                "</div>";

                email.DateTime = DateTime.Now;

                int emailId =   await edp.AddEmail(email);

                //edp.AddEmail()
                ViewData["Content"] = email.Content;
                ViewData["EmailId"] = emailId;
                ViewData["Subject"] = email.Subject;
                ViewData["From"] = email.From;
                ViewData["SettingsId"] = email.SettingsId;
            }


            return View();
        }
    }
}