using System;
using Vaccination.App.Mapping;
using Vaccination.Domain.Entities;

namespace Vaccination.App.CQRS.DTO
{
	public class InoculationDto : BaseMapFrom<Inoculation>
	{
		public Vaccine Vaccine { get; set; }
		public bool HasConsent { get; set; }
		public DateTime Date { get; set; }
	}
}
