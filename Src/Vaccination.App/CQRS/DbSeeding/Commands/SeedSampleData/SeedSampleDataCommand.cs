using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Vaccination.App.CQRS.DbSeeding.Commands.SeedSampleData
{
	public class SeedSampleDataCommand : IRequest
	{
	}

	public class SeedSampleDataCommandHandler : RequestHandler<SeedSampleDataCommand>
	{
		public SeedSampleDataCommandHandler(IServiceProvider services) 
			: base(services)
		{
		}

		public override async Task<Unit> Handle(SeedSampleDataCommand request, CancellationToken cancellationToken)
		{
			var seeder = new SampleDataSeeder(_dbContext);
			await seeder.SeedAllAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
