using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Vaccination.App;
using Vaccination.App.CQRS.Patients.Commands.UpdatePatient;
using Vaccination.EF;
using Vaccination.Infastructure;
using WebUI.Inner.Filters;
using WebUI.Inner.ViewResults;

namespace Vaccination
{
	public class Startup
	{
		private readonly IHostEnvironment _hostEnvironment;

		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
		{
			Configuration = configuration;
			_hostEnvironment = hostEnvironment;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.TryAddSingleton<IActionResultExecutor<ViewResult>, MyViewResultExecutor>();

			services
				.AddInfastructure(Configuration)
				.AddEntityFramework(Configuration)
				.AddApp();

			var mvcBuilder = services.AddControllersWithViews(opt =>
			{
				opt.Filters.Add(typeof(RemoveStringsSpacesRedundancyFilter));
			});

			mvcBuilder.AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<UpdatePatientValidator>());

			if (_hostEnvironment.IsDevelopment())
			{
				mvcBuilder.AddRazorRuntimeCompilation();
			}
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
		{
			SetCultures("ru-RU");

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Errors/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.ConfigureEntityFramework(env.IsDevelopment());

			app.UseStatusCodePagesWithReExecute("/Errors/PageNotFound/{0}");

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}

		private void SetCultures(string cult)
		{
			var cultureInfo = new CultureInfo(cult);
			CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
			CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
		}
	}
}
