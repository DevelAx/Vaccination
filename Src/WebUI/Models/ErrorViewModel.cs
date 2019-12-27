using System;

namespace Vaccination.Models
{
	public class ErrorViewModel
	{
		public string RequestId { get; set; }

		public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

		public string Message { get; }

		public ErrorViewModel(string message)
		{
			Message = message;
		}
	}
}
