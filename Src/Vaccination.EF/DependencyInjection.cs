using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Vaccination.Infastructure.Config.Sections;
using Vaccination.Interfaces;

namespace Vaccination.EF
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
		{
			string connectionString = configuration.GetConnectionString("DefaultConnection");

			services.AddDbContext<VaccinationDbContext>(builder =>
			{
				builder.UseNpgsql(connectionString);
			});

			services.AddScoped<IVaccinationDbContext>(provider => provider.GetService<VaccinationDbContext>());

			return services;
		}
	}
}
