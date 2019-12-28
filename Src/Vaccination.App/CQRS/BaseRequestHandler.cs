using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vaccination.Infastructure.Errors;
using Vaccination.Interfaces;

namespace Vaccination.App.CQRS
{
	public abstract class RequestHandler<TRequest, TData> : BaseRequestHandler<TRequest, RequestResult<TData>>
		where TRequest : IRequest<RequestResult<TData>>
	{
		public RequestHandler(IServiceProvider services)
			: base(services)
		{ }

		protected RequestResult<TData> Result(TData result) => new RequestResult<TData>(result);
		protected RequestResult<TData> Error(VaccinationAppException error) => new RequestResult<TData>(error);
	}

	public abstract class RequestHandler<TRequest> : BaseRequestHandler<TRequest, RequestResult>
		where TRequest : IRequest<RequestResult>
	{
		public RequestHandler(IServiceProvider services)
			: base(services)
		{ }

		protected RequestResult Result => RequestResult.Empty;
	}

	#region BaseRequestHandler

	public abstract class BaseRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
		where TRequest : IRequest<TResponse>
	{
		protected readonly IVaccinationDbContext _dbContext;
		protected readonly IMapper _mapper;
		protected readonly IMediator _mediator;

		public BaseRequestHandler(IServiceProvider services)
		{
			_dbContext = services.GetRequiredService<IVaccinationDbContext>();
			_mapper = services.GetRequiredService<IMapper>();
			_mediator = services.GetRequiredService<IMediator>();
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

	#endregion BaseRequestHandler

	public interface IRequestResult : IRequest<RequestResult> { }
	public interface IRequestResult<TData> : IRequest<RequestResult<TData>> { }
}
