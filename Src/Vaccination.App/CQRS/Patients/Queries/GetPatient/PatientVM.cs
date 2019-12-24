using AutoMapper;
using System.Collections.Generic;
using Vaccination.App.CQRS.DTO;
using Vaccination.Domain.Entities;

namespace Vaccination.App.CQRS.Patients.Queries.GetPatient
{
	public class PatientVM : PatientDto
	{
		public ICollection<InoculationDto> Inoculations { get; set; }

		public override int InoculationsCount { get => Inoculations.Count; }

		public override void Mapping(Profile profile)
		{
			profile.CreateMap<Patient, PatientVM>()
				.ForMember(p => p.BirthDate, opt => opt.MapFrom(s => s.BirthDate.ToLongDateString()));
		}
	}
}
