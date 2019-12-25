using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Vaccination.Domain.Entities;

namespace Vaccination.Interfaces
{
	public interface IVaccinationDbContext
	{
		DbSet<Vaccine> Vaccines { get; set; }

		DbSet<Inoculation> Inoculations { get; set; }

		DbSet<Patient> Patients { get; set; }

		Task UpdateAsync<TEntity>(Expression<Func<TEntity, TEntity>> update, Expression<Func<TEntity, bool>> filter) 
			where TEntity : _Base.Entity;

		Task<int> SaveChangesAsync(CancellationToken cancellationToken);

		Task MigrateAsync();
	}
}
