using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Vaccination.Domain.Entities;
using Vaccination.Infastructure.Utils;
using Vaccination.Interfaces;

namespace Vaccination.App.CQRS.DbSeeding.Commands.SeedSampleData
{
	public class SampleDataSeeder
	{
		private readonly IVaccinationDbContext _context;
		private Vaccine[] _vaccines;

		public SampleDataSeeder(IVaccinationDbContext dbContext)
		{
			_context = dbContext;
		}

		public async Task SeedAllAsync(CancellationToken cancelToken)
		{
			if (await _context.Patients.AnyAsync())
				return;

			SeedVaccines(cancelToken);
			SeedPatients(cancelToken);
			await ((DbContext)_context).SaveChangesAsync(cancelToken);
		}

		private void SeedVaccines(CancellationToken cancellationToken)
		{
			_vaccines = new Vaccine[]
			{
				new Vaccine { Name = "Эджерикс" },
				new Vaccine { Name = "Вианвак" },
				new Vaccine { Name = "АКДС" },
				new Vaccine { Name = "БЦЖ" },
			};

			_context.Vaccines.AddRange(_vaccines);
		}

		private void SeedPatients(CancellationToken cancellationToken)
		{
			Random rnd = new Random(Environment.TickCount);
			RandomDate randomDate = new RandomDate(1950, DateTime.Now.Year - 1);
			int n = 0;

			var patients = new Patient[]
			{
				NewPatient("Максимов", "Сергей", "Иванович", "М"),
				NewPatient("Иванова", "Любовь", "Андреевна", "Ж"),
				NewPatient("Касьянов", "Михаил", "Валерьевич", "М"),
				NewPatient("Путин", "Владимир", "Владимирович", "М"),
				NewPatient("Жириновский", "Владимир", "Вольфович", "М"),
				NewPatient("Филатова", "Татьяна", "Николаевна", "Ж"),
				NewPatient("Гончаров", "Иван", "Александрович", "М"),
				NewPatient("Державин", "Гавриил", "Романович", "М"),
				NewPatient("Достоевский", "Фёдор", "Михайлович", "М"),
				NewPatient("Замятин", "Евгений", "Иванович", "М"),
				NewPatient("Карамзин", "Николай", "Михайлович", "М"),
				NewPatient("Тютчев", "Фёдор", "Иванович", "М"),
				NewPatient("Лермонтов", "Михаил", "Юрьевич", "М"),
				NewPatient("Ломоносов", "Михаил", "Васильевич", "М"),
				NewPatient("Некрасов", "Николай", "Николаевич", "М"),
				NewPatient("Мамин-Сибиряк", "Дмитрий", "Наркисович", "М"),
				NewPatient("Паустовский", "Константин", "Георгиевич", "М"),
				NewPatient("Пушкин", "Александр", "Сергеевич", "М"),
				NewPatient("Толстой", "Алексей", "Николаевич", "М"),
				NewPatient("Тургенев", "Иван", "Сергеевич", "М"),
				NewPatient("Цветаева", "Марина", "Ивановна", "Ж"),
			};

			_context.Patients.AddRange(patients);

			Patient NewPatient(string lastName, string firstName, string patronymic, string sex)
			{
				return new Patient
				{
					BirthDate = randomDate.Next(),
					LastName = lastName,
					FirstName = firstName,
					Patronymic = patronymic,
					Sex = sex,
					InsuranceNumber = NewInsuranceNumber(),
					Inoculations = NewInoculations()
				};
			}

			string NewInsuranceNumber()
			{
				return "12345678" + n++.ToString("000");
			}

			ICollection<Inoculation> NewInoculations()
			{
				var randomDate = new RandomDate(2000, DateTime.Now.Year - 1);
				int n = rnd.Next(0, _vaccines.Length);
				var inoculations = new List<Inoculation>(n);

				for (int i = 0; i < n; i++)
				{
					var inoculation = new Inoculation
					{
						HasConsent = (rnd.Next(0, 2) == 1),
						Date = randomDate.Next(),
						VaccineId = _vaccines[i].Id,
					};

					inoculations.Add(inoculation);
				}

				return inoculations;
			}
		}
	}
}
