using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Vaccination.App.CQRS.Patients.Queries.GetPatients;
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

		public async Task<IActionResult> Index()
		{
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
