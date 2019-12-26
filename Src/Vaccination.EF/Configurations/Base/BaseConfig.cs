using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Vaccination.Domain.Entities;

namespace Vaccination.EF.Configurations
{
	public abstract class BaseConfig<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : _Base.Entity
	{
		public virtual void Configure(EntityTypeBuilder<TEntity> builder)
		{
			SetTablesPluralNames(builder);
		}

		private void SetTablesPluralNames(EntityTypeBuilder<TEntity> builder)
		{
			string entityName = typeof(TEntity).Name;
			Debug.WriteLine("Entity type name: " + entityName);
			string ending = "s";

			if (char.ToLowerInvariant(entityName[entityName.Length - 1]) == 's')
				ending = "es";

			builder.ToTable(entityName + ending);
		}
	}
}
