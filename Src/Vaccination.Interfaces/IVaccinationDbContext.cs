using Microsoft.EntityFrameworkCore;
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

		Task<int> SaveChangesAsync(CancellationToken cancellationToken);
		Task MigrateAsync();
	}
}
