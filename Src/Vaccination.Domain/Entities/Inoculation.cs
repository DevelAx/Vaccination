using System;

namespace Vaccination.Domain.Entities
{
	public class Inoculation : _Base.AuditableEntity
	{
		public Guid PatientId { get; set; }
		public Guid VaccineId { get; set; }
		public Vaccine Vaccine { get; set; }
		public bool HasConsent { get; set; }
		public DateTime Date { get; set; }
	}
}
