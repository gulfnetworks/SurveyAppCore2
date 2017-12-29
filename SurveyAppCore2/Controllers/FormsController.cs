using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SurveyAppCore2.Controllers
{
    public class FormsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Survey()
        {
            return View();
        }

        public IActionResult Advanced()
        {
            return View();
        }
    }
}