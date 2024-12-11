using Microsoft.AspNetCore.Mvc;
using ServiceLifeTime.Models;
using ServiceLifeTime.Services;
using System.Diagnostics;
using System.Text;

namespace ServiceLifeTime.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISingleToneService SingleToneService1;
        private readonly ISingleToneService SingleToneService2;
        private readonly IScopedService scopedService1;
        private readonly IScopedService scopedService2;
        private readonly ITransientService transientService1;
        private readonly ITransientService transientService2;

        public HomeController(ISingleToneService singleToneService1, ISingleToneService singleToneService2,
            IScopedService scopedService1, IScopedService scopedService2, ITransientService transientService1,
            ITransientService transientService2)
        {
            SingleToneService1 = singleToneService1;
            SingleToneService2 = singleToneService2;
            this.scopedService1 = scopedService1;
            this.scopedService2 = scopedService2;
            this.transientService1 = transientService1;
            this.transientService2 = transientService2;
        }

        public string Index()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Single 1    :: {SingleToneService1.GetGuid()}");
            stringBuilder.AppendLine($"Single 2    :: {SingleToneService2.GetGuid()}\n\n");
            stringBuilder.AppendLine($"Scoped 1    :: {scopedService1.GetGuid()}");
            stringBuilder.AppendLine($"Scoped 2    :: {scopedService2.GetGuid()}\n\n");
            stringBuilder.AppendLine($"Transient 1 :: {transientService1.GetGuid()}");
            stringBuilder.AppendLine($"Transient 2 :: {transientService2.GetGuid()}\n\n");


            return stringBuilder.ToString();

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
