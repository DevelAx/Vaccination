using System;
using System.Collections.Generic;
using System.Text;
using Vaccination.Domain.Entities.Proto;

namespace Vaccination.Domain.Entities
{
	public abstract class _Base : BaseGenerics<Guid>
	{
		// Once you decide to change the type of `Id` field, here is the only place you have to change it.
	}
}
