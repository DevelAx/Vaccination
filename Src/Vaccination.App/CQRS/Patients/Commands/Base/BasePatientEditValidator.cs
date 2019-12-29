using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Text.RegularExpressions;
using Vaccination.Infastructure.Utils;

namespace Vaccination.App.CQRS.Patients.Commands.Base
{
	public class BasePatientEditValidator : BasePatientEditValidator<BasePatientEditVM> { }

	public abstract class BasePatientEditValidator<T> : AbstractValidator<T>
		where T : BasePatientEditVM
	{
		private readonly static DateTime MIN_BIRTH_DATE = new DateTime(1899, 1, 1);

		private static DateTime MAX_BIRTH_DATE => DateTime.Now;

		private readonly static Regex _regex = new Regex(@"^\d{11}$");

		public BasePatientEditValidator()
		{
			RuleFor(x => x.BirthDate).Required().DatesRange(MIN_BIRTH_DATE, MAX_BIRTH_DATE);
			RuleFor(x => x.LastName).Required();
			RuleFor(x => x.FirstName).Required();
			RuleFor(x => x.Sex).Required();
			RuleFor(x => x.InsuranceNumber)
				.Required()
				.Custom(ValidateInsuranceNumber);
		}

		private void ValidateInsuranceNumber(string input, CustomContext context)
		{
			if (input == null)
				return; // The message is set in `Required()` but still propogated here.

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
	}
}
