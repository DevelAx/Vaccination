using FluentValidation;

namespace Vaccination.App
{
	public static class FluidValidationExtensions
	{
		public static IRuleBuilderOptions<T, TProperty> Required<T, TProperty>
			(this IRuleBuilder<T, TProperty> ruleBuilder, string message = "Поле обязательно к заполнению")
		{
			return ruleBuilder.NotEmpty().WithMessage(message);
		}

		public static IRuleBuilderOptions<T, string> LengthEx<T>
			(this IRuleBuilder<T, string> ruleBuilder, int exactLength, string message = "Значение поля должно быть из {1} символов")
		{
			return ruleBuilder.Length(exactLength).WithMessage(message);
		}
	}
}
