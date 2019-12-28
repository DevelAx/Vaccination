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
	public class UpdatePatientCommand : IRequestResult
	{
		public EditPatientVM Patient { get; }

		public UpdatePatientCommand(EditPatientVM vm)
		{
			Patient = vm;
		}
	}

	public class UpdatePaitientCommandHandler : RequestHandler<UpdatePatientCommand>
	{
		public UpdatePaitientCommandHandler(IServiceProvider services) 
			: base(services)
		{
		}

		public override async Task<RequestResult> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
		{
			var vm = request.Patient;

			foreach(var inoculation in vm.Inoculations.Where(i => i.IsDeleted).ToList())
			{
				vm.Inoculations.Remove(inoculation);

				if (Guid.Empty != inoculation.Id) // When an inoculation was created and then deleted in one edit session.
					_dbContext.Inoculations.Remove(new Inoculation { Id = inoculation.Id });
			}

			Patient patient = new Patient { Id = vm.Id };
			_dbContext.Patients.Attach(patient).Property(m=>m.Patronymic).IsModified = true;
			_mapper.Map(request.Patient, patient);
			await _dbContext.SaveChangesAsync(cancellationToken);

			return Result;
		}
	}
}
