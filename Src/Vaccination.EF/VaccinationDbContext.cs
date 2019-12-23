using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vaccination.Domain.Entities;
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
					patient.NormalizedFullName = string.Join(" ", patient.LastName, patient.FirstName, patient.Patronymic).TrimEnd().ToUpper().Replace('Ё', 'E');
				}
			}

			return base.SaveChangesAsync(cancellationToken);
		}

		public async Task MigrateAsync()
		{
			await Database.MigrateAsync();
		}
	}
}
