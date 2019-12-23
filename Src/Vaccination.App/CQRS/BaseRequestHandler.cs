using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vaccination.Interfaces;

namespace Vaccination.App.CQRS
{
	public abstract class BaseRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
		where TRequest : IRequest<TResponse>
	{
		protected readonly IVaccinationDbContext _dbContext;
		protected readonly IMapper _mapper;

		public BaseRequestHandler(IServiceProvider services)
		{
			_dbContext = services.GetRequiredService<IVaccinationDbContext>();
			_mapper = services.GetRequiredService<IMapper>();
		}

		public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
	}

	public abstract class BaseRequestHandler<TRequest> : BaseRequestHandler<TRequest, Unit>
		where TRequest : IRequest<Unit>
	{
		public BaseRequestHandler(IServiceProvider services) 
			: base(services)
		{
		}
	}
}
