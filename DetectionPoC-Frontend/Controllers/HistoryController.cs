using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DetectionPoC_Frontend.Controllers
{
    public class HistoryController : Controller
    {
        public IActionResult Index(string resourceId)
        {
            return View();
        }
    }
}