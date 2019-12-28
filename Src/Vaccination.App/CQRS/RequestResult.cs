using Vaccination.Infastructure.Errors;

namespace Vaccination.App.CQRS
{
	public class RequestResult
	{
		public static RequestResult Empty { get; } = new RequestResult();
		
		public VaccinationAppException Error { get; }
		
		public virtual object Data { get; }

		public bool HasError => Error != null;

		public RequestResult() { }

		public RequestResult(VaccinationAppException error)
		{
			Error = error;
		}

		public RequestResult(object data)
		{
			Data = data;
		}
	}

	public class RequestResult<TData> : RequestResult
	{
		public RequestResult(TData data)
			:base(data)
		{
		}

		public RequestResult(VaccinationAppException error)
			:base(error)
		{ }
	}
}
