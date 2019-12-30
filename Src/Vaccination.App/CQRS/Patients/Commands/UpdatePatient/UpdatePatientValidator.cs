using FluentValidation;
using System;
using Vaccination.App.CQRS.Patients.Commands.Base;
using Vaccination.App.CQRS.Patients.Queries.GetEditPaitient;
using Vaccination.Interfaces;

namespace Vaccination.App.CQRS.Patients.Commands.UpdatePatient
{
	public class UpdatePatientValidator : BasePatientEditValidator<EditPatientVM>
	{
		public UpdatePatientValidator(IVaccinationDbContext dbContext) 
			: base(dbContext)
		{
			RuleFor(x => x.Id).Required();
			RuleForEach(x => x.Inoculations).SetValidator(x => new UpdateInoculationValidator(x));
		}

		private class UpdateInoculationValidator : AbstractValidator<EditInoculationDto>
		{
			public UpdateInoculationValidator(EditPatientVM parent)
			{
				When(i => !i.IsDeleted, () =>
				  {
					  RuleFor(x => x.VaccineId).Required();
					  RuleFor(x => x.Date).DatesRange(parent.BirthDate.Value, DateTime.Now);
				  });
			}
		}
	}
}
