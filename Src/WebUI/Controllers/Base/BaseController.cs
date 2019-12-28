using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net;
using Vaccination.Infastructure.Errors;
using Vaccination.Models;

namespace WebUI.Controllers.Base
{
	public abstract class BaseController<T> : Controller
	{
		private IMediator _mediator;
		private readonly IServiceProvider _services;
		protected readonly ILogger<T> _log;

		protected IMediator Mediator => _mediator ??= _services.GetService<IMediator>();

		public BaseController(IServiceProvider services)
		{
			_services = services;
			_log = _services.GetService<ILogger<T>>();
		}

		protected IActionResult Error(Exception error, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
		{
			_log.LogError(error, "Error");

			var errorModel = new ErrorViewModel
			{
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
			};

			if (error is VaccinationAppException appError)
			{
				errorModel.Message = appError.Message;
			}

			HttpContext.Response.StatusCode = (int)statusCode;
			return View("Error", errorModel);
		}
	}
}