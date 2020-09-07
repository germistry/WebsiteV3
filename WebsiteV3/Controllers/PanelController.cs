﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteV3.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class PanelController : Controller
    {
        private readonly ILogger<PanelController> _logger;

        public PanelController(ILogger<PanelController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}