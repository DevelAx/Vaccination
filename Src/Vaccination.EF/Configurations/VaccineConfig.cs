

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vaccination.Domain.Entities;

namespace Vaccination.EF.Configurations
{
	public class VaccineConfig : BaseConfig<Vaccine>
	{
		public override void Configure(EntityTypeBuilder<Vaccine> builder)
		{
			base.Configure(builder);
			builder.Property(p => p.Name).IsRequired();
		}
	}
}
