using System.ComponentModel.DataAnnotations;

namespace Vaccination.App.AuxiliaryTypes
{
	public enum Sexes
	{
		[Display(Name = "М")]
		Male = 0,

		[Display(Name = "Ж")]
		Female = 1
	}
}
