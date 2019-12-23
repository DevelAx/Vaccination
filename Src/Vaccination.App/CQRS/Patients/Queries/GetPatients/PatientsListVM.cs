using System.Collections.Generic;

namespace Vaccination.App.CQRS.Patients.Queries.GetPatients
{
	public class PatientsListVM
	{
		public IList<PatientDto> Patients { get; set; }
		public int CurrentPage { get; set; }
		public int PagesCount { get; set; }
	}
}
