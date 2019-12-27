using Microsoft.AspNetCore.Builder;

namespace Vaccination.EF
{
	public static class Configuration
	{
		public static IApplicationBuilder ConfigureEntityFramework(this IApplicationBuilder app, bool isDevelopment)
		{
			if (isDevelopment)
			{
				app.UseDatabaseErrorPage();
			}

			return app;
		}
	}
}
