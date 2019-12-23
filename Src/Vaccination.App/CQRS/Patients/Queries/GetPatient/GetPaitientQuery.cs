using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vaccination.App.CQRS.DTO;
using Vaccination.App.CQRS.Exceptions;

namespace Vaccination.App.CQRS.Patients.Queries.GetPatient
{
	public class GetPaitientQuery : IRequest<PatientVM>
	{
		public int PatientId { get; }

		public GetPaitientQuery(int patientId)
		{
			PatientId = patientId;
		}
	}

	public class GetPaitientQueryHandler : BaseRequestHandler<GetPaitientQuery, PatientVM>
	{
		public GetPaitientQueryHandler(IServiceProvider services)
			: base(services)
		{
		}

		public override async Task<PatientVM> Handle(GetPaitientQuery request, CancellationToken cancellationToken)
		{
			var patientVM = await _dbContext.Patients
				.ProjectTo<PatientVM>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync(p => p.IntId == request.PatientId);

			if (patientVM == null)
				throw new PatientNotFoundException("Пациент не найден.");

			patientVM.Inoculations = patientVM.Inoculations.OrderBy(i => i.Date).ToList();
			return patientVM;
		}
	}
}
