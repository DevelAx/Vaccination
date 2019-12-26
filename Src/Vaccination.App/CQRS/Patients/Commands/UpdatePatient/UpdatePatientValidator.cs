using FluentValidation;
using System;
using System.Collections.Generic;
using Vaccination.App.CQRS.Patients.Queries.GetEditPaitient;

namespace Vaccination.App.CQRS.Patients.Commands.UpdatePatient
{
	public class UpdatePatientValidator : AbstractValidator<EditPatientVM>
	{
		private readonly static DateTime MIN_BIRTH_DATE = new DateTime(1899, 1, 1);
		private static DateTime MAX_BIRTH_DATE => DateTime.Now;

		public UpdatePatientValidator()
		{
			RuleFor(x => x.Id).Required();
			RuleFor(x => x.BirthDate).DatesRange(MIN_BIRTH_DATE, MAX_BIRTH_DATE);
			RuleFor(x => x.LastName).Required();
			RuleFor(x => x.FirstName).Required();
			RuleFor(x => x.InsuranceNumber).Required().Matches(@"^\d{11}$").WithMessage("СНИЛС должен состоять из 11 цифр");
			RuleForEach(x => x.Inoculations).SetValidator(x=> new UpdateInoculationValidator(x));
		}

		private class UpdateInoculationValidator : AbstractValidator<EditInoculationDto>
		{
			public UpdateInoculationValidator(EditPatientVM parent)
			{
				When(i => !i.IsDeleted, () =>
				{
					RuleFor(x => x.VaccineId).Required();
					RuleFor(x => x.Date).DatesRange(parent.BirthDate, DateTime.Now);
				});
			}
		}
	}
}
