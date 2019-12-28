using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vaccination.Domain.Entities;

namespace Vaccination.App.CQRS.Patients.Queries.GetAllVaccines
{
	public class AllVaccinesQuery : IRequestResult<Vaccine[]>
	{
	}

	public class AllVaccinesQueryHandler : RequestHandler<AllVaccinesQuery, Vaccine[]>
	{
		public AllVaccinesQueryHandler(IServiceProvider services)
			: base(services)
		{
		}

		public override async Task<RequestResult<Vaccine[]>> Handle(AllVaccinesQuery request, CancellationToken cancellationToken)
		{
			var allVaccines = await _dbContext.Vaccines
				.OrderBy(v => v.Name)
				.ToArrayAsync(cancellationToken);

			return Result(allVaccines);
		}
	}
}
