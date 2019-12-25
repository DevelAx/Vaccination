using FluentValidation;
using System;
using Vaccination.App.CQRS.Patients.Queries.GetEditPaitient;

namespace Vaccination.App.CQRS.Patients.Commands.UpdatePatient
{
	public class UpdatePatientValidator : AbstractValidator<EditPatientVM>
	{
		private readonly static DateTime MIN_BIRTH_DATE = new DateTime(1899, 1, 1);
		private static DateTime MAX_BIRTH_DATE => DateTime.Now;

		public UpdatePatientValidator()
		{
			RuleFor(x => x.IntId).Required();

			RuleFor(x => x.BirthDate)
				.Must(BeActualDate)
				.WithMessage($"Дата рождения должна быть в диапазоне от {MIN_BIRTH_DATE.ToShortDateString()} до {MAX_BIRTH_DATE.ToShortDateString()}");

			RuleFor(x => x.LastName).Required();
			RuleFor(x => x.FirstName).Required();
			RuleFor(x => x.InsuranceNumber).Required().Matches(@"^\d{11}$").WithMessage("СНИЛС должен состоять из 11 цифр");
			RuleFor(x => x.Sex).Required();
		}

		private bool BeActualDate(DateTime date)
		{
			if (date.Equals(default))
				return false;

			if (date < MIN_BIRTH_DATE || date > MAX_BIRTH_DATE)
				return false;

			return true;
		}


	}
}
