using AutoMapper;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Vaccination.App.Mapping;
using Vaccination.Domain.Entities;

namespace Vaccination.App.CQRS.Patients.Queries.GetEditPaitient
{
	public class EditInoculationDto : BaseMapFrom<Inoculation>
	{
		[DisplayName("Вакцина")]
		public Guid? VaccineId { get; set; }

		[DisplayName("Дата постановки")]
		public DateTime? Date { get; set; }

		[DisplayName("Наличие согласия пациента")]
		public bool HasConsent { get; set; }

		public bool IsDeleted { get; set; }

		public bool IsActual()
		{
			return !IsDeleted && Id != Guid.Empty;
		}

		public override void Mapping(Profile profile)
		{
			base.Mapping(profile);
			profile.CreateMap<EditInoculationDto, Inoculation>();
		}
	}
}
