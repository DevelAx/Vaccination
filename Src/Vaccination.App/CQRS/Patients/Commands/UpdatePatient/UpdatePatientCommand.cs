using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vaccination.App.CQRS.Exceptions;
using Vaccination.App.CQRS.Patients.Queries.GetEditPaitient;
using Vaccination.Domain.Entities;

namespace Vaccination.App.CQRS.Patients.Commands.UpdatePatient
{
	public class UpdatePatientCommand : IRequest
	{
		public EditPatientVM Patient { get; }

		public UpdatePatientCommand(EditPatientVM vm)
		{
			Patient = vm;
		}
	}

	public class UpdatePaitientCommandHandler : BaseRequestHandler<UpdatePatientCommand>
	{
		public UpdatePaitientCommandHandler(IServiceProvider services) 
			: base(services)
		{
		}

		public override async Task<Unit> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
		{
			var tempPatient = await _dbContext.Patients
				.Where(p => p.IntId == request.Patient.IntId)
				.Select(p => new { p.Id })
				.FirstOrDefaultAsync();

			if (tempPatient == null)
				throw new PatientNotFoundException();

			Patient patient = _mapper.Map<Patient>(request.Patient);
			patient.Id = tempPatient.Id;

			_dbContext.Patients.Update(patient);
			await _dbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
