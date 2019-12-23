namespace Vaccination.App.CQRS.Patients.Queries.GetPatients
{
	public class PatientDto : DTO.PatientDto
	{
		public int InoculationsCount { get; set; }
	}
}
