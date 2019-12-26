using System;

namespace Vaccination.Domain.Entities.Proto
{
	public abstract class BaseGenerics<TId>
	{
		public abstract class Entity
		{
			public TId Id { get; set; }
		}

		public abstract class AuditableEntity : Entity
		{
			public DateTime Created { get; set; }
			public DateTime? LastModified { get; set; }
		}
	}
}
