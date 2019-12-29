using System;
using System.Threading;
using System.Threading.Tasks;
using Vaccination.Domain.Entities;

namespace Vaccination.App.CQRS.Patients.Commands.CreatePatient
{
	public class CreatePatientCommand : IRequestResult<CreatePatientVM>
	{
		public CreatePatientVM Patient { get; }

		public CreatePatientCommand(CreatePatientVM vm)
		{
			Patient = vm;
		}
	}

	public class CreatePatientCommandHandler : RequestResultHandler<CreatePatientCommand, CreatePatientVM>
	{
		public CreatePatientCommandHandler(IServiceProvider services) 
			: base(services)
		{
		}

		public override async Task<RequestResult<CreatePatientVM>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
		{
			CreatePatientVM vm = request.Patient;

			try
			{
				Patient patient = _mapper.Map<Patient>(vm);
				var entry = _dbContext.Patients.Add(patient);
				await _dbContext.SaveChangesAsync(cancellationToken);
				vm = _mapper.Map<CreatePatientVM>(entry.Entity);
			}
			catch (Exception exc)
			{
				string message = "Создание пациента завершилось ошибкой.";
				return Error(message, exc);
			}

			return Result(vm);
		}
	}
}
