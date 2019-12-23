using System;

namespace Vaccination.Infastructure.Utils
{
	public class RandomDate
	{
		private static readonly Random _rnd = new Random(Environment.TickCount);
		private readonly DateTime _start, _end;

		public RandomDate(int minYear, int maxYear)
		{
			if (minYear > maxYear)
				throw new IndexOutOfRangeException($"`{nameof(minYear)}` can't be more than `{nameof(maxYear)}`");

			_start = new DateTime(minYear, 1, 1);
			_end = new DateTime(maxYear, 12, 31);
		}

		public DateTime Next()
		{
			int range = (_end - _start).Days;
			return _start.AddDays(_rnd.Next(range));
		}
	}
}
