using AutoMapper;
using Vaccination.Domain.Entities;

namespace Vaccination.App.Mapping
{
	public abstract class BaseMapFrom<TSource> : _Base.Entity
	{
		public virtual void Mapping(Profile profile)
		{
			profile.CreateMap(typeof(TSource), GetType());
		}
	}
}
