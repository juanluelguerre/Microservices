using ElGuerre.Microservices.Ordering.Api.Domain;
using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates;
using ElGuerre.Microservices.Ordering.Api.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Shared.Extensions
{
	static class MediatorExtensions
	{
		public static async Task DispatchDomainEventsAsync(this IMediator mediator, OrderingContext ctx)
		{
			var domainEntities = ctx.ChangeTracker
				.Entries<Entity>()
				.Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

			var domainEvents = domainEntities
				.SelectMany(x => x.Entity.DomainEvents)
				.ToList();

			domainEntities.ToList()
				.ForEach(entity => entity.Entity.ClearDomainEvents());

			var tasks = domainEvents
				.Select(async (domainEvent) => {
					await mediator.Publish(domainEvent);
				});

			await Task.WhenAll(tasks);
		}
	}
}
