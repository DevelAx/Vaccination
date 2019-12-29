using AutoMapper;
using Vaccination.App.CQRS.Patients.Commands.Base;

namespace Vaccination.App.CQRS.Patients.Commands.CreatePatient
{
	public class CreatePatientVM : BasePatientEditVM
	{
		public override void Mapping(Profile profile)
		{
			Mapping<CreatePatientVM>(profile);
		}
	}
}
