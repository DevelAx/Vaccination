using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vaccination.Domain.Entities;

namespace Vaccination.EF.Configurations
{
	public class PatientConfig : BaseConfig<Patient>
	{
		public override void Configure(EntityTypeBuilder<Patient> builder)
		{
			base.Configure(builder);

			builder.Property(p => p.LastName).IsRequired();
			builder.Property(p => p.FirstName).IsRequired();
			builder.Property(p => p.NormalizedFullName).IsRequired();
			builder.Property(p => p.NormalizedLastName).IsRequired();
			builder.Property(p => p.NormalizedFirstName).IsRequired();
			builder.Property(p => p.Sex).IsRequired();
			builder.Property(p => p.InsuranceNumber).IsRequired().HasMaxLength(11);
			builder.HasIndex(p => p.InsuranceNumber).IsUnique();
		}
	}
}
