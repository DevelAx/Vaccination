using System;
using System.Collections.Generic;
using System.Text;
using Vaccination.Infastructure.Errors;

namespace Vaccination.App.CQRS.Exceptions
{
	public class PatientNotFoundException : VaccinationAppException
	{
		public PatientNotFoundException(string message) : base(message) { }
	}
}
