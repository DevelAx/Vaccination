using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Vaccination.Infastructure.Errors;
using Vaccination.Models;

namespace WebUI.Controllers
{
	[AllowAnonymous]
	public class ErrorsController : Controller
	{
		private readonly ILogger<ErrorsController> _log;

		public ErrorsController(ILogger<ErrorsController> log)
		{
			_log = log;
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			string userMessage = string.Empty;
			var excHandler = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

			if (excHandler != null)
			{
				_log.LogError(excHandler.Error, excHandler.Path);

				if (excHandler.Error is VaccinationAppException error)
				{
					userMessage = error.Message;
				}
			}
			else
			{
				_log.LogError("Unknown error occured.");
			}

			var vm = new ErrorViewModel(userMessage)
			{
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
			};

			return View(vm);
		}

		public IActionResult PageNotFound(int statusCode)
		{
			return View(statusCode);
		}
	}
}
