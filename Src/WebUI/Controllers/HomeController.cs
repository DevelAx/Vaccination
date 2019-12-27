﻿using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Vaccination.Infastructure.Errors;
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
		[AllowAnonymous]
		public IActionResult Error()
		{
			string userMessage = "системная ошибка, попробуйте открыть другую страницу";
			var excHandler = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

			if (excHandler != null)
			{
				if (excHandler.Error is VaccinationAppException error)
				{
					userMessage = error.Message;
				}

				_log.LogError(excHandler.Error, excHandler.Path);
			}

			var vm = new ErrorViewModel(userMessage)
			{
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
			};

			return View(vm);
		}
	}
}
