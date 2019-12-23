using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

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
	}
}