using System;

namespace Vaccination.Infastructure.Errors
{
	public class VaccinationAppException : Exception
	{
		public VaccinationAppException(string message) 
			: base(message) { }

		public VaccinationAppException(string message, Exception innerException)
			:base(message, innerException) { }
	}
}
