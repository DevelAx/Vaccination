using Vaccination.Infastructure.Errors;

namespace Vaccination.App.CQRS
{
	public class RequestResult
	{
		public static RequestResult Empty { get; } = new RequestResult();
		public VaccinationAppException Error { get; }

		public bool HasError => Error != null;

		public RequestResult() { }

		public RequestResult(VaccinationAppException error)
		{
			Error = error;
		}
	}

	public class RequestResult<TData> : RequestResult
	{
		public TData Data { get; }

		public RequestResult(TData data)
		{
			Data = data;
		}

		public RequestResult(VaccinationAppException error)
			:base(error)
		{ }
	}
}
