using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vaccination.App.CQRS.Exceptions;

namespace Vaccination.App.CQRS.Patients.Queries.GetPatient
{
	public class PaitientQuery : IRequestResult<PatientVM>
	{
		public Guid PatientId { get; }

		public PaitientQuery(Guid patientId)
		{
			PatientId = patientId;
		}
	}

	public class PaitientQueryHandler : RequestHandler<PaitientQuery, PatientVM>
	{
		public PaitientQueryHandler(IServiceProvider services)
			: base(services)
		{ }

		public override async Task<RequestResult<PatientVM>> Handle(PaitientQuery request, CancellationToken cancellationToken)
		{
			var vm = await _dbContext.Patients
				.ProjectTo<PatientVM>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync(p => p.Id == request.PatientId, cancellationToken);

			if (vm == null)
				return Error(new PatientNotFoundException());

			vm.Inoculations = vm.Inoculations.OrderBy(i => i.Date).ToList();
			return Result(vm);
		}
	}
}
