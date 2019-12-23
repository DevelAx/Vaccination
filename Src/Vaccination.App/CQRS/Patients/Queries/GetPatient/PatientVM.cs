using System.Collections.Generic;
using System.ComponentModel;
using Vaccination.App.CQRS.DTO;

namespace Vaccination.App.CQRS.Patients.Queries.GetPatient
{
	public class PatientVM : PatientDto
	{
		[DisplayName("Кол-во прививок")]
		public int InoculationsCount => Inoculations.Count;


		public List<InoculationDto> Inoculations { get; set; }
	}
}
