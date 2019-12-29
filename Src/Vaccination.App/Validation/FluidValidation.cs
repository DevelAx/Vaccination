using FluentValidation;
using System;

namespace Vaccination.App
{
	public static class FluidValidation
	{
		public const string RequiredMessage = "Поле обязательно к заполнению";

		public static IRuleBuilderOptions<T, TProperty> Required<T, TProperty>
			(this IRuleBuilder<T, TProperty> ruleBuilder, string message = RequiredMessage)
		{
			return ruleBuilder.NotEmpty().WithMessage(message);
		}

		public static IRuleBuilderOptions<T, string> LengthEx<T>
			(this IRuleBuilder<T, string> ruleBuilder, int exactLength, string message = "Значение поля должно быть из {1} символов")
		{
			return ruleBuilder.Length(exactLength).WithMessage(message);
		}

		public static IRuleBuilderOptions<T, DateTime> DatesRange<T>
			(this IRuleBuilder<T, DateTime> builder, DateTime minDate, DateTime maxDate, string message = "Дата должна быть в диапазоне от {0} до {1}")
		{
			message = string.Format(message, minDate.ToShortDateString(), maxDate.ToShortDateString());
			return builder.Must(BeActualDate).WithMessage(message);

			bool BeActualDate(DateTime date)
			{
				if (date.Equals(default))
					return false;

				if (date < minDate || date > maxDate)
					return false;

				return true;
			}
		}

		public static IRuleBuilderOptions<T, DateTime?> DatesRange<T>
			(this IRuleBuilder<T, DateTime?> builder, DateTime minDate, DateTime maxDate, string message = "Дата должна быть в диапазоне от {0} до {1}")
		{
			message = string.Format(message, minDate.ToShortDateString(), maxDate.ToShortDateString());
			return builder.Must(BeActualDate).WithMessage(message);

			bool BeActualDate(DateTime? date)
			{
				if (date.Equals(default))
					return false;

				if (date < minDate || date > maxDate)
					return false;

				return true;
			}
		}
	}
}
