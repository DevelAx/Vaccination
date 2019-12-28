using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vaccination.App.CQRS.Exceptions;
using Vaccination.App.CQRS.Patients.Queries.GetAllVaccines;

namespace Vaccination.App.CQRS.Patients.Queries.GetEditPaitient
{
	public class EditPaitientQuery : IRequestResult<EditPatientVM>
	{
		public Guid PatientId { get; }

		public EditPaitientQuery(Guid patientId)
		{
			PatientId = patientId;
		}
	}

	public class EditPaitientQueryHandler : RequestResultHandler<EditPaitientQuery, EditPatientVM>
	{
		public EditPaitientQueryHandler(IServiceProvider services) 
			: base(services)
		{
		}

		public override async Task<RequestResult<EditPatientVM>> Handle(EditPaitientQuery request, CancellationToken cancellationToken)
		{
			var vm = await _dbContext.Patients
				.ProjectTo<EditPatientVM>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync(p => p.Id == request.PatientId, cancellationToken);

			if (vm == null)
				return Error(new PatientNotFoundException());

			vm.Inoculations = vm.Inoculations.OrderBy(i => i.Date).ToList();
			vm.AllVaccines = await _mediator.Send(new AllVaccinesQuery());
			return Result(vm);
		}
	}
}
