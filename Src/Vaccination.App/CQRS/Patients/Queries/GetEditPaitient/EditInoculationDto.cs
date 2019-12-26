using AutoMapper;
using System;
using System.ComponentModel;
using Vaccination.App.Mapping;
using Vaccination.Domain.Entities;

namespace Vaccination.App.CQRS.Patients.Queries.GetEditPaitient
{
	public class EditInoculationDto : BaseMapFrom<Inoculation>
	{
		public Guid Id { get; set; }

		[DisplayName("Вакцина")]
		public Guid VaccineId { get; set; }

		[DisplayName("Дата постановки")]
		public DateTime Date { get; set; }

		[DisplayName("Наличие согласия пациента")]
		public bool HasConsent { get; set; }

		public bool IsDeleted { get; set; }

		public override void Mapping(Profile profile)
		{
			base.Mapping(profile);
			profile.CreateMap<EditInoculationDto, Inoculation>();
		}
	}
}
