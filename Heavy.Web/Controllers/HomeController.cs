using System.Diagnostics;
using Heavy.Web.Data;
using Heavy.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using Heavy.Web.Models;
using Microsoft.Extensions.Logging;

namespace Heavy.Web.Controllers
{
    //[LogResourceFilter]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //[LogResourceFilter]
        public IActionResult Index()
        {
            _logger.LogInformation(MyLogEventIds.HomePage, "Visiting Home Index ...");

            //throw new Exception("Something happened!");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult MyError()
        {
            return View();
        }
    }
}
