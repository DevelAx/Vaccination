using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Vaccination.Domain.Entities;
using Vaccination.Domain.Entities.Proto;
using Vaccination.Interfaces;

namespace Vaccination.EF
{
	public class VaccinationDbContext : DbContext, IVaccinationDbContext
	{
		private readonly ILoggerFactory _loggerFactory;

		public DbSet<Vaccine> Vaccines { get; set; }
		public DbSet<Inoculation> Inoculations { get; set; }
		public DbSet<Patient> Patients { get; set; }

		public VaccinationDbContext(DbContextOptions<VaccinationDbContext> options, ILoggerFactory loggerFactory)
			: base(options)
		{
			_loggerFactory = loggerFactory;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);

			optionsBuilder
				.UseLoggerFactory(_loggerFactory)
				.EnableSensitiveDataLogging();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(VaccinationDbContext).Assembly, t => !t.IsAbstract);
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
		{
			foreach (var entry in ChangeTracker.Entries())
			{
				if (entry.Entity is _Base.AuditableEntity auditable)
				{
					switch (entry.State)
					{
						case EntityState.Added:
							auditable.Created = DateTime.UtcNow;
							break;
						case EntityState.Modified:
							auditable.LastModified = DateTime.Now;
							break;
					}
				}

				if (entry.Entity is Patient patient)
				{
					patient.NormalizedLastName = Normalize(patient.LastName);
					patient.NormalizedFirstName = Normalize(patient.FirstName);
					patient.NormalizedPatronymic = Normalize(patient.Patronymic);

					patient.NormalizedFullName = string
						.Join(" ", patient.NormalizedLastName, patient.NormalizedFirstName, patient.NormalizedPatronymic)
						.TrimEnd();
				}
			}

			return base.SaveChangesAsync(cancellationToken);
		}

		private string Normalize(string str)
		{
			return str?.ToUpper().Replace('Ё', 'Е');
		}

		public async Task MigrateAsync()
		{
			await Database.MigrateAsync();
		}

		public Task UpdateAsync<TEntity>(Expression<Func<TEntity, TEntity>> update, Expression<Func<TEntity, bool>> filter) 
			where TEntity : BaseGenerics<Guid>.Entity
		{
			if (filter == null)
				throw new NullReferenceException("Filter cannot be empty, otherwise all the records will be updated.");

			IQueryable<TEntity> query = Set<TEntity>();

			if (filter != null)
				query = query.Where(filter);

			return query.UpdateFromQueryAsync(update);
		}
	}
}
