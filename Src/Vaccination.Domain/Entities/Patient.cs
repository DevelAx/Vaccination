using System;
using System.Collections.Generic;

namespace Vaccination.Domain.Entities
{
	public class Patient : _Base.AuditableEntity
	{
		public Patient()
		{
			return;
		}

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Patronymic { get; set; }
		public string NormalizedFullName { get; set; }
		public DateTime BirthDate { get; set; }
		public string Sex { get; set; }
		public string InsuranceNumber { get; set; }

		public ICollection<Inoculation> Inoculations { get; set; } = new HashSet<Inoculation>();
	}
}
