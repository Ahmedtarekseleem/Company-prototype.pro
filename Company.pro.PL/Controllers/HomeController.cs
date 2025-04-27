using Company.pro.PL.Models;
using Company.pro.PL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;

namespace Company.pro.PL.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IScopedService scopedService01;
        private readonly IScopedService scopedService02;
        private readonly ITransentService transentService01;
        private readonly ITransentService transentService02;
        private readonly ISengeltonService sengeltonService01;
        private readonly ISengeltonService sengeltonService02;

        public HomeController(
            ILogger<HomeController> logger,
            IScopedService scopedService01,
            IScopedService scopedService02,
            ITransentService transentService01,
            ITransentService transentService02,
            ISengeltonService sengeltonService01,
            ISengeltonService sengeltonService02)
        {
            _logger = logger;
            this.scopedService01 = scopedService01;
            this.scopedService02 = scopedService02;
            this.transentService01 = transentService01;
            this.transentService02 = transentService02;
            this.sengeltonService01 = sengeltonService01;
            this.sengeltonService02 = sengeltonService02;
        }

        public string TestLifeTime()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"scopedService01 :: {scopedService01.GetGuid()}\n");
            builder.Append($"scopedService02 :: {scopedService02.GetGuid()}\n\n");
            builder.Append($"transentService01 :: {transentService01.GetGuid()}\n");
            builder.Append($"transentService02 :: {transentService02.GetGuid()}\n\n");
            builder.Append($"sengeltonService01 :: {sengeltonService01.GetGuid()}\n");
            builder.Append($"sengeltonService02 :: {sengeltonService02.GetGuid()}\n\n");
            return builder.ToString();
        }
        public IActionResult Index()
        {
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
    }
}
