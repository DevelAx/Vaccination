using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vaccination.Infastructure.Errors;
using Vaccination.Interfaces;

namespace Vaccination.App.CQRS
{
	public abstract class RequestResultHandler<TRequest, TData> : RequestHandler<TRequest, RequestResult<TData>>
		where TRequest : IRequest<RequestResult<TData>>
	{
		public RequestResultHandler(IServiceProvider services)
			: base(services)
		{ }

		protected RequestResult<TData> Result(TData result) => new RequestResult<TData>(result);
		protected RequestResult<TData> Error(VaccinationAppException error) => new RequestResult<TData>(error);
		protected RequestResult<TData> Error(string error) => new RequestResult<TData>(new VaccinationAppException(error));
	}

	public abstract class RequestResultHandler<TRequest> : RequestHandler<TRequest, RequestResult>
		where TRequest : IRequest<RequestResult>
	{
		public RequestResultHandler(IServiceProvider services)
			: base(services)
		{ }

		protected RequestResult Result => RequestResult.Empty;
		protected RequestResult Error(VaccinationAppException error) => new RequestResult(error);
		protected RequestResult Error(string error) => new RequestResult(new VaccinationAppException(error));
	}

	#region BaseRequestHandler

	public abstract class RequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
		where TRequest : IRequest<TResponse>
	{
		protected readonly IVaccinationDbContext _dbContext;
		protected readonly IMapper _mapper;
		protected readonly IMediator _mediator;
		protected readonly ILogger _logger;

		public RequestHandler(IServiceProvider services)
		{
			_dbContext = services.GetRequiredService<IVaccinationDbContext>();
			_mapper = services.GetRequiredService<IMapper>();
			_mediator = services.GetRequiredService<IMediator>();
			_logger = services.GetRequiredService<ILogger<TRequest>>();
		}

		public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
	}

	public abstract class RequestHandler<TRequest> : RequestHandler<TRequest, Unit>
		where TRequest : IRequest<Unit>
	{
		public RequestHandler(IServiceProvider services)
			: base(services)
		{ }
	}

	#endregion BaseRequestHandler

	public interface IRequestResult : IRequest<RequestResult> { }
	public interface IRequestResult<TData> : IRequest<RequestResult<TData>> { }
}
