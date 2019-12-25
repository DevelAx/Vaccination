using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vaccination.App.CQRS.Exceptions;

namespace Vaccination.App.CQRS.Patients.Queries.GetEditPaitient
{
	public class GetEditPaitientQuery : IRequest<EditPatientVM>
	{
		public int PatientId { get; }

		public GetEditPaitientQuery(int patientId)
		{
			PatientId = patientId;
		}
	}

	public class GetEditPaitientQueryHandler : BaseRequestHandler<GetEditPaitientQuery, EditPatientVM>
	{
		public GetEditPaitientQueryHandler(IServiceProvider services) 
			: base(services)
		{
		}

		public override async Task<EditPatientVM> Handle(GetEditPaitientQuery request, CancellationToken cancellationToken)
		{
			var vm = await _dbContext.Patients
				.ProjectTo<EditPatientVM>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync(p => p.IntId == request.PatientId, cancellationToken);

			if (vm == null)
				throw new PatientNotFoundException();

			return vm;
		}
	}
}
