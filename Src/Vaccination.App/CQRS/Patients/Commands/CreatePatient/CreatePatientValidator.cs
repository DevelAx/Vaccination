using Vaccination.App.CQRS.Patients.Commands.Base;
using Vaccination.Interfaces;

namespace Vaccination.App.CQRS.Patients.Commands.CreatePatient
{
	public class CreatePatientValidator : BasePatientEditValidator<CreatePatientVM>
	{
		public CreatePatientValidator(IVaccinationDbContext dbContext) 
			: base(dbContext)
		{ }
	}
}
