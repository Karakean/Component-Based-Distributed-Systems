using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Library.Web.Configuration;
using Microsoft.AspNetCore.Mvc;
using Library.Web.Models;
using Library.Web.Utilities;
using Microsoft.Extensions.Options;

namespace Library.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly EnvironmentConfig _environmentConfiguration;

        public HomeController(IOptions<EnvironmentConfig> configuration)
        {
            _environmentConfiguration = configuration.Value;
        }

        public async Task<IActionResult> Index()
        {
            var rentedBooks = await RequestHandler.MakeRequest<List<LibraryResource>>($@"{_environmentConfiguration.LibraryWebApiServiceHost}/api/library/rented/");
            
            var model = rentedBooks.Select(resource => $"{resource?.Id} - {resource?.Book?.Title}, {resource?.Book?.Author}");
            return View(model.ToList());
        }


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}