using System.Collections.Generic;
using Vaccination.App.CQRS.DTO;

namespace Vaccination.App.CQRS.Patients.Queries.FindPatients
{
	public class PatientsListVM
	{
		public int TotalPatientsCount { get; set; }
		public IList<PatientDto> Patients { get; set; }
		public object[] Pages { get; set; }
		public int SearchId { get; set; }
		public int PageNumber { get; set; }
	}
}
