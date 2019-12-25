using AutoMapper;

namespace Vaccination.App.Mapping
{
	public abstract class BaseMapFrom<TSource>
	{
		public virtual void Mapping(Profile profile)
		{
			profile.CreateMap(typeof(TSource), GetType());
		}
	}
}
