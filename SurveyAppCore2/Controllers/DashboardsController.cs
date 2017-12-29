using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace SurveyAppCore2.Controllers
{
  
    public class DashboardsController : Controller
    {
        [Authorize]
        public IActionResult Dashboard_1()
        {
            return View();
        }

        public IActionResult Dashboard_2()
        {
            return View();
        }

        public IActionResult Dashboard_3()
        {
            return View();
        }

        public IActionResult Dashboard_4()
        {
            return View();
        }

        public IActionResult Dashboard_4_1()
        {
            return View();
        }

        public IActionResult Dashboard_5()
        {
            return View();
        }
    }
}