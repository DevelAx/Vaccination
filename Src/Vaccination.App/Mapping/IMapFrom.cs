using AutoMapper;

namespace Vaccination.App.Mapping
{
	public interface IMapFrom<T>
	{
		void Mapping(Profile profile)
		{
			profile.CreateMap(typeof(T), GetType());
		}
	}
}
