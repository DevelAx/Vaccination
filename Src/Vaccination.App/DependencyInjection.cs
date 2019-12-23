using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Vaccination.App
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApp(this IServiceCollection services)
		{
			var executingAssembly = Assembly.GetExecutingAssembly();
			services.AddAutoMapper(executingAssembly);
			services.AddMediatR(executingAssembly);
			return services;
		}
	}
}
