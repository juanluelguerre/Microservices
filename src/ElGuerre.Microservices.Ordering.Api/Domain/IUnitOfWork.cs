using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Domain
{
	public interface IUnitOfWork : IDisposable
	{
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
		Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken));
	}
}
