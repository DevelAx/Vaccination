using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vaccination.App.CQRS.DTO;
using Vaccination.Domain.Entities;
using Vaccination.Infastructure.Config.Sections;

namespace Vaccination.App.CQRS.Patients.Queries.FindPatients
{
	public class FindPatientsQuery : IRequest<PatientsListVM>
	{
		public string FullName { get; }
		public string InsuranceNumber { get; }
		public int SearchId { get; }
		public int Page { get; }

		public FindPatientsQuery(string fullName, string insuranceNumber, int searchId, int page)
		{
			FullName = fullName;
			InsuranceNumber = insuranceNumber;
			SearchId = searchId;
			Page = page;
		}
	}

	public class FindPatientsQueryHandler : BaseRequestHandler<FindPatientsQuery, PatientsListVM>
	{
		public int ItemsPerPage { get; }

		public FindPatientsQueryHandler(IServiceProvider services, IOptions<AppSettings> settings)
			: base(services)
		{
			ItemsPerPage = settings.Value.ItemsPerPage;
		}

		public override async Task<PatientsListVM> Handle(FindPatientsQuery request, CancellationToken cancellationToken)
		{
			int skip = ItemsPerPage * request.Page;
			int take = ItemsPerPage;

			string fullName = request.FullName?.ToUpper().Replace('Ё', 'Е');
			string insuranceNumber = request.InsuranceNumber;
			string[] names = null;

			int startWithMax = 3;
			IQueryable<Patient> query = _dbContext.Patients;

			if (!string.IsNullOrWhiteSpace(fullName))
			{
				names = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

				for (int i = 0; i < names.Length; i++)
				{
					string name = names[i];

					if (name.Length < startWithMax)
					{
						query = query.Where(p =>
							p.NormalizedLastName.StartsWith(name) ||
							p.NormalizedFirstName.StartsWith(name) ||
							p.NormalizedPatronymic.StartsWith(name)
						);
					}
					else
					{
						query = query.Where(p => p.NormalizedFullName.Contains(name));
					}
				}
			}

			if (!string.IsNullOrWhiteSpace(insuranceNumber))
			{
				if (insuranceNumber.Length < startWithMax)
				{
					query = query.Where(p => p.InsuranceNumber.StartsWith(insuranceNumber));
				}
				else
				{
					query = query.Where(p => p.InsuranceNumber.Contains(insuranceNumber));
				}
			}

			int patientsCount = await query.CountAsync();

			query = query
					.OrderBy(p => p.LastName).ThenBy(p => p.FirstName).ThenBy(p => p.Patronymic)
					.ThenBy(p => p.BirthDate)
					.ThenBy(p => p.Sex == "М" ? 1 : 0);

			var patients = await query
				.Skip(skip)
				.Take(take)
				.ProjectTo<PatientDto>(_mapper.ConfigurationProvider)
				.ToListAsync(cancellationToken);

			foreach(var patient in patients)
			{
				if (names != null)
				{
					foreach(var name in names)
					{
						if (name.Length < startWithMax)
						{
							patient.LastName = HightlightIfStartsWith(patient.NormalizedLastName, patient.LastName, name);
							patient.FirstName = HightlightIfStartsWith(patient.NormalizedFirstName, patient.FirstName, name);
							patient.Patronymic = HightlightIfStartsWith(patient.NormalizedPatronymic, patient.Patronymic, name);
						}
						else
						{
							patient.LastName = HightlightIfContains(patient.NormalizedLastName, patient.LastName, name);
							patient.FirstName = HightlightIfContains(patient.NormalizedFirstName, patient.FirstName, name);
							patient.Patronymic = HightlightIfContains(patient.NormalizedPatronymic, patient.Patronymic, name);
						}	
					}
				}

				if (!string.IsNullOrEmpty(insuranceNumber))
				{
					if (insuranceNumber.Length < startWithMax)
					{
						patient.InsuranceNumber = HightlightIfStartsWith(patient.InsuranceNumber, patient.InsuranceNumber, insuranceNumber);
					}
					else
					{
						patient.InsuranceNumber = HightlightIfContains(patient.InsuranceNumber, patient.InsuranceNumber, insuranceNumber);
					}
				}
			}

			int pagesCount = patientsCount / ItemsPerPage + (patientsCount % ItemsPerPage > 0 ? 1 : 0);
			var pages = Enumerable.Range(1, pagesCount).Select(n => new { Number = n }).ToArray();

			var vm = new PatientsListVM
			{
				Patients = patients,
				SearchId = request.SearchId,
				PageNumber = request.Page,
				Pages = pages
			};

			return vm;
		}

		private string HightlightIfStartsWith(string normalizedName, string name, string token)
		{
			if (normalizedName.StartsWith(token))
			{
				return "<u>" + name.Substring(0, token.Length) + "</u>" + name.Substring(token.Length);
			}

			return name;
		}

		private string HightlightIfContains(string normalizedName, string name, string token)
		{
			int pos = normalizedName.IndexOf(token);

			if (pos == -1)
				return name;

			string p0 = name.Substring(0, pos);
			string p1 = name.Substring(pos, token.Length);
			string p2 = name.Substring(Math.Min(name.Length, pos + token.Length));
			StringBuilder builder = new StringBuilder(p0);
			builder.Append("<u>").Append(p1).Append("</u>").Append(p2);
			return builder.ToString();
		}
	}
}
