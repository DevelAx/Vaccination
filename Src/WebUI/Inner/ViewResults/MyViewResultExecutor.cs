using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using Vaccination.App.CQRS;
using Vaccination.Infastructure.Errors;
using Vaccination.Models;

namespace WebUI.Inner.ViewResults
{
	public class MyViewResultExecutor : ViewResultExecutor
	{
		private readonly IWebHostEnvironment _env;

		public MyViewResultExecutor(IOptions<MvcViewOptions> viewOptions, IHttpResponseStreamWriterFactory writerFactory, ICompositeViewEngine viewEngine, ITempDataDictionaryFactory tempDataFactory, DiagnosticListener diagnosticListener, ILoggerFactory loggerFactory, IModelMetadataProvider modelMetadataProvider, IWebHostEnvironment env)
			: base(viewOptions, writerFactory, viewEngine, tempDataFactory, diagnosticListener, loggerFactory, modelMetadataProvider)
		{
			_env = env;
		}

		public override ViewEngineResult FindView(ActionContext actionContext, ViewResult viewResult)
		{
			if (viewResult.ViewData.Model is RequestResult myResult)
			{
				if (myResult.HasError)
				{
					var errorModel = new ErrorViewModel
					{
						RequestId = Activity.Current?.Id ?? actionContext.HttpContext.TraceIdentifier
					};

					if (_env.IsDevelopment())
					{
						errorModel.DevelopmentInfo = myResult.Error.ToString();
					}
					else if (myResult.Error is VaccinationAppException appError)
					{
						errorModel.Message = appError.Message;
					}


					viewResult.ViewData.Model = errorModel;
					viewResult.ViewName = "Error";
				}
				else
				{
					viewResult.ViewData.Model = myResult.Data;
				}
			}

			return base.FindView(actionContext, viewResult);
		}
	}
}
