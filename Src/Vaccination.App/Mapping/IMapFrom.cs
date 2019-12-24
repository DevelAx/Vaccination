using AutoMapper;

namespace Vaccination.App.Mapping
{
	public interface IMapFrom<TSource>
	{
		void Mapping(Profile profile)
		{
			profile.CreateMap(typeof(TSource), GetType());
		}
	}
}
