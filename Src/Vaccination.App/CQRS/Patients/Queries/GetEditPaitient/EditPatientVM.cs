using AutoMapper;
using System.Collections.Generic;
using Vaccination.App.CQRS.Patients.Commands.Base;
using Vaccination.App.CQRS.Patients.Commands.CreatePatient;

namespace Vaccination.App.CQRS.Patients.Queries.GetEditPaitient
{
	public class EditPatientVM : BasePatientEditVM
	{
		public IList<EditInoculationDto> Inoculations { get; set; } = new List<EditInoculationDto>();

		public override void Mapping(Profile profile)
		{
			Mapping<EditPatientVM>(profile);
		}
	}
}
