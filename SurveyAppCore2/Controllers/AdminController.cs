﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SurveyAppCore2.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CompanyList()
        {
            return View();
        }

        public IActionResult UpdateCompany(string Id)
        {
            ViewData["SettingsId"] = Id;
            return View();
        }
    }
}