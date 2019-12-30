using FluentValidation;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Vaccination.Infastructure.Utils;
using Vaccination.Interfaces;

namespace Vaccination.App.CQRS.Patients.Commands.Base
{
	public class BasePatientEditValidator : BasePatientEditValidator<BasePatientEditVM>
	{
		public BasePatientEditValidator(IVaccinationDbContext dbContext)
			: base(dbContext) { }
	}

	public abstract class BasePatientEditValidator<T> : AbstractValidator<T>
		where T : BasePatientEditVM
	{
		private readonly static DateTime MIN_BIRTH_DATE = new DateTime(1899, 1, 1);

		private static DateTime MAX_BIRTH_DATE => DateTime.Now;

		private readonly static Regex _regex = new Regex(@"^\d{11}$");
		private readonly IVaccinationDbContext _dbContext;

		public BasePatientEditValidator(IVaccinationDbContext dbContext)
		{
			_dbContext = dbContext;

			RuleFor(x => x.BirthDate).Required().DatesRange(MIN_BIRTH_DATE, MAX_BIRTH_DATE);
			RuleFor(x => x.LastName).Required();
			RuleFor(x => x.FirstName).Required();
			RuleFor(x => x.Sex).Required();
			RuleFor(x => x.InsuranceNumber)
				.Required()
				.Custom(ValidateInsuranceNumber)
				.MustAsync(BeUniqueInsuranceNumber).WithMessage("СНИЛС неуникален (принадлежит другому пациенту)");
		}

		private async Task<bool> BeUniqueInsuranceNumber(BasePatientEditVM vm, string input, PropertyValidatorContext context, CancellationToken cancellationToken)
		{
			if (input == null)
				return false; // The message is set in `Required()` but still propogated here.

			bool notUnique = await _dbContext.Patients.AnyAsync(p => p.InsuranceNumber == input && p.Id != vm.Id, cancellationToken);

			return !notUnique;
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
			string number = input.Substring(0, input.Length - checkSumLen);

			if (int.Parse(number) <= InsuranceNumber.MinNumber)
			{
				context.AddFailure($"Первые 9 цифр СНИЛС должны быть больше числа {InsuranceNumber.MinNumber}");
				return;
			}

			int correctChecksum = InsuranceNumber.GetChecksum(number);

			if (correctChecksum != checksum)
			{
				context.AddFailure("Некорректный СНИЛС: контрольное число не соответствует контрольной сумме номера");
				return;
			}
		}
	}
}
