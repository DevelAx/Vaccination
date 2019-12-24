using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Vaccination.Models;
using WebUI.Controllers.Base;

namespace Vaccination.Controllers
{
	public class HomeController : BaseController<HomeController>
	{
		public HomeController(IServiceProvider services) 
			: base(services)
		{
		}

		public IActionResult Index()
		{
			_log.LogDebug(MethodBase.GetCurrentMethod().Name);
			return View();
		}

		public IActionResult Privacy()
		{
			_log.LogDebug(MethodBase.GetCurrentMethod().Name);
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			_log.LogDebug(MethodBase.GetCurrentMethod().Name);
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
