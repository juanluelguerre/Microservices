using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Domain
{
	public interface IRepository<T> where T : IAggregateRoot
	{
		IUnitOfWork UnitOfWork { get; }
	}
}
