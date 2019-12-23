using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Vaccination.App.CQRS.DbSeeding.Commands.SeedSampleData;
using Vaccination.Interfaces;

namespace Vaccination
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			//CreateHostBuilder(args).Build().Run();
			var host = CreateHostBuilder(args).Build();

			using(var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				ILogger<Program> log = null;

				try
				{
					log = services.GetRequiredService<ILogger<Program>>();

					log.LogInformation("DB migration...");
					var dbContext = services.GetRequiredService<IVaccinationDbContext>();
					await dbContext.MigrateAsync();

					log.LogInformation("DB sample data seeding...");
					var mediator = services.GetRequiredService<IMediator>();
					await mediator.Send(new SeedSampleDataCommand());

					log.LogInformation("DB preparations completed.");
				}
				catch(Exception exc)
				{
					Console.Error.WriteLine(exc);
					log.LogError(exc, "An error occurred while migrating or initializing the database.");
				}
			}

			await host.RunAsync();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>()
					.ConfigureAppConfiguration((hostCtx, cfg) =>
					{
						cfg.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
					})
					.ConfigureLogging(logging =>
					{
						//logging.AddFilter(DbLoggerCategory.Query.Name, LogLevel.Debug);
						//logging.AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Debug);
						logging.AddFilter(DbLoggerCategory.Database.Transaction.Name, LogLevel.Debug);
						logging.AddFilter(DbLoggerCategory.Update.Name, LogLevel.Debug);
					});
				});
	}
}
