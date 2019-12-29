using System.Linq;

namespace Vaccination.Infastructure.Utils
{
	///////////////////////////
	// https://insur-portal.ru/pension/kontrolnoe-chislo-snils
	// СНИЛС: NNN-NNN-NNN KK
	//
	public class InsuranceNumber
	{
		public const int MinNumber = 1001998; // Вычисления подходят для номеров больше 001-001-998;
		public const string MinNumberText = "001001998";

		/// <summary>
		/// The `input` argument requirments:
		/// СНИЛС-номер должен состоять из 9 цифр (без контрольной суммы).
		/// СНИЛС-номер должен быть больше значение номера должн обыть 001-001-998.
		/// </summary>
		public static int GetChecksum(string input)
		{
			// Для того чтобы определить, является ли указанное контрольное значение верным, необходимо:
			// каждый элемент N умножить на порядковый номер, исчисляемый в противоположном порядке (для первого элемента N порядковый номер будет 9, для второго – 8 и т.д.);
			// полученные значения суммировать между собой.
			int sum = Enumerable
				.Range(0, input.Length)
				.Select(n => (input[n] - '0') * (input.Length - n))
				.Aggregate((sum, next) => sum + next);

			// Полученное значение сравнивается относительно цифры 100. Если оно:
			// больше 101, то его необходимо нацело разделить на 101.
			if (sum > 101)
				sum %= 101;

			// меньше 100, то контрольное число равно самой сумме;
			if (sum < 100)
				return sum;

			// равно 100 или 101, то контрольное число – 00;
			return 0;
		}
	}
}
