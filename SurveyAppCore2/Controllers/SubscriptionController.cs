﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SurveyAppCore2.Controllers
{
    public class SubscriptionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}