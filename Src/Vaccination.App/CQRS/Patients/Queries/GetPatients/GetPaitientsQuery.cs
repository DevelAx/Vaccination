using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Vaccination.App.CQRS.Patients.Queries.GetPatients
{
	public class GetPaitientsQuery : IRequest<PatientsListVM>
	{
		public int Page { get; }
		public int ItemsPerPage { get; }

		public GetPaitientsQuery(int page, int itemsPerPage)
		{
			Page = page;
			ItemsPerPage = itemsPerPage;
		}
	}

	public class GetPaitientsQueryHandler : BaseRequestHandler<GetPaitientsQuery, PatientsListVM>
	{
		public GetPaitientsQueryHandler(IServiceProvider services)
			: base(services)
		{
		}

		public override async Task<PatientsListVM> Handle(GetPaitientsQuery request, CancellationToken cancellationToken)
		{
			int skip = request.ItemsPerPage * request.Page;
			int take = request.ItemsPerPage;
			
			int patientsCount = await _dbContext.Patients.CountAsync();

			var patients = await _dbContext.Patients
				.ProjectTo<PatientDto>(_mapper.ConfigurationProvider)
				.OrderBy(p => p.LastName).ThenBy(p => p.FirstName).ThenBy(p => p.Patronymic)
				.ThenBy(p => p.BirthDate)
				.ThenBy(p => p.Sex == "М" ? 1 : 0) // Just sex discrimination.
				.Skip(skip)
				.Take(take)
				.ToListAsync();

			var vm = new PatientsListVM
			{
				Patients = patients,
				CurrentPage = request.Page,
				PagesCount = patientsCount / request.ItemsPerPage + (patientsCount % request.ItemsPerPage > 0 ? 1 : 0)
			};

			return vm;
		}
	}
}
