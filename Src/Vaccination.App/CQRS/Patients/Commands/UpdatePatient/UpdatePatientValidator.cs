using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Text.RegularExpressions;
using Vaccination.App.CQRS.Patients.Queries.GetEditPaitient;
using Vaccination.Infastructure.Utils;

namespace Vaccination.App.CQRS.Patients.Commands.UpdatePatient
{
	public class UpdatePatientValidator : AbstractValidator<EditPatientVM>
	{
		private readonly static DateTime MIN_BIRTH_DATE = new DateTime(1899, 1, 1);
		
		private static DateTime MAX_BIRTH_DATE => DateTime.Now;

		private readonly static Regex _regex = new Regex(@"^\d{11}$");

		public UpdatePatientValidator()
		{
			RuleForEach(x => x.Inoculations).SetValidator(x => new UpdateInoculationValidator(x));
			RuleFor(x => x.Id).Required();
			RuleFor(x => x.BirthDate).DatesRange(MIN_BIRTH_DATE, MAX_BIRTH_DATE);
			RuleFor(x => x.LastName).Required();
			RuleFor(x => x.FirstName).Required();
			RuleFor(x => x.InsuranceNumber)
				.Required()
				.Custom(ValidateInsuranceNumberChecksum);
		}

		private void ValidateInsuranceNumberChecksum(string input, CustomContext context)
		{
			if (!_regex.Match(input).Success)
			{
				context.AddFailure("СНИЛС должен состоять из 11 цифр");
				return;
			}

			// КК – контрольное число;
			int checkSumLen = 2;
			int checksum = int.Parse(input.Substring(input.Length - checkSumLen, checkSumLen));
			
			// NNN-NNN-NNN – номер;
			input = input.Substring(0, input.Length - checkSumLen);

			if (int.Parse(input) <= InsuranceNumber.MinNumber)
			{
				context.AddFailure($"Первые 9 цифр СНИЛС должны быть больше числа {InsuranceNumber.MinNumber}");
				return;
			}

			int correctChecksum = InsuranceNumber.GetChecksum(input);

			if (correctChecksum != checksum)
				context.AddFailure("Некорректный СНИЛС: контрольное число не соответствует контрольной сумме номера");
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
