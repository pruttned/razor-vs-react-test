using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Jering.Javascript.NodeJS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RazorApp.Models;
using RazorApp.Services;

namespace RazorApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INewsService _newsService;
        private readonly INodeJSService _nodeJSService;

        public HomeController(ILogger<HomeController> logger, INewsService newsService, INodeJSService nodeJSService)
        {
            _logger = logger;
            this._newsService = newsService;
            this._nodeJSService = nodeJSService;
        }

        public IActionResult Index()
        {
            return View(new IndexViewModel
            {
                News = _newsService.GetAll(10)
            });
        }

        public async Task<IActionResult> IndexReact()
        {
            var model = new IndexViewModel
            {
                News = _newsService.GetAll(10)
            };
            var result = await _nodeJSService.InvokeFromFileAsync<string>("./render.js", args: new object[] { "/Home/IndexReact", "http://localhost:5000", model });
            return Content(result, "text/html");
        }

        public async Task<IActionResult> IndexReact2()
        {
            var result = await _nodeJSService.InvokeFromFileAsync<string>("./render.js", args: new object[] { "/Home/IndexReact", "http://localhost:5000" });
            return Content(result, "text/html");
        }

        public async Task<IActionResult> DetailReact(string id)
        {
            var result = await _nodeJSService.InvokeFromFileAsync<string>("./render.js", args: new[] { $"/Home/DetailReact/{id}", "http://localhost:5000" });
            return Content(result, "text/html");
        }

        public IActionResult Detail(string id)
        {
            return View(_newsService.Get(id));
        }

        public IActionResult GetAll()
        {
            return Ok(new IndexViewModel
            {
                News = _newsService.GetAll(10)
            });
        }

        public IActionResult GetDetail(string alias)
        {
            return Ok(_newsService.Get(alias));
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

    public class IndexViewModel
    {
        public List<NewsListViewModel> News { get; set; }
    }
}
