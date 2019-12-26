using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vaccination.App.CQRS.Exceptions;

namespace Vaccination.App.CQRS.Patients.Queries.GetPatient
{
	public class PaitientQuery : IRequest<PatientVM>
	{
		public Guid PatientId { get; }

		public PaitientQuery(Guid patientId)
		{
			PatientId = patientId;
		}
	}

	public class PaitientQueryHandler : BaseRequestHandler<PaitientQuery, PatientVM>
	{
		public PaitientQueryHandler(IServiceProvider services)
			: base(services)
		{
		}

		public override async Task<PatientVM> Handle(PaitientQuery request, CancellationToken cancellationToken)
		{
			var vm = await _dbContext.Patients
				.ProjectTo<PatientVM>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync(p => p.Id == request.PatientId, cancellationToken);

			if (vm == null)
				throw new PatientNotFoundException();

			vm.Inoculations = vm.Inoculations.OrderBy(i => i.Date).ToList();
			return vm;
		}
	}
}
