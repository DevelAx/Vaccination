using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vaccination.Infastructure.Config.Sections;

namespace Vaccination.Infastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfastructure(this IServiceCollection services, IConfiguration config)
		{
			services.ConfigureSection<AppSettings>(config);
			return services;
		}
	}
}
