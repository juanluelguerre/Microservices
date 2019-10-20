using ElGuerre.Microservices.Ordering.Api.Domain;
using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Customers;
using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Orders;
using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Customers;
using ElGuerre.Microservices.Ordering.Api.Infrastructure.Repositories;
using ElGuerre.Microservices.Shared.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Infrastructure
{
	public class OrderingContext : DbContext, IUnitOfWork
	{
		public const string DEFAULT_SCHEMA = "Ordering";
		private readonly IMediator _mediator;

		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderItem> OrderItems { get; set; }
		public DbSet<PaymentMethod> Payments { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<CardType> CardTypes { get; set; }
		public DbSet<OrderStatus> OrderStatus { get; set; }

		public OrderingContext() : base() { }        

        public OrderingContext(DbContextOptions<OrderingContext> options, IMediator mediator) : base(options)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));			
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.ApplyConfigurationsFromAssembly(typeof(Startup).Assembly);
		}

		public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			// Dispatch Domain Events collection. 
			// Choices:
			// A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
			// side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
			// B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
			// You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
			await _mediator.DispatchDomainEventsAsync(this);

			// After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
			// performed through the DbContext will be committed
			var result = await base.SaveChangesAsync(cancellationToken);

			return true;
		}

		public class CatalogContextDesignFactory : IDesignTimeDbContextFactory<OrderingContext>
		{
			public OrderingContext CreateDbContext(string[] args)
			{
				IConfigurationRoot configuration = new ConfigurationBuilder()
				   .SetBasePath(Directory.GetCurrentDirectory())
				   .AddUserSecrets<Startup>()
				   .AddJsonFile("appsettings.json")
				   .Build();

				var builder = new DbContextOptionsBuilder<OrderingContext>();

				builder.UseSqlServer(configuration.GetConnectionString("DataBaseConnection"));

				return new OrderingContext(builder.Options, new NoMediator());
			}

			class NoMediator : IMediator
			{
				public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default(CancellationToken)) where TNotification : INotification
				{
					return Task.CompletedTask;
				}

				public Task Publish(object notification, CancellationToken cancellationToken = default)
				{
					return Task.CompletedTask;
				}

				public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default(CancellationToken))
				{
					return Task.FromResult<TResponse>(default(TResponse));
				}

				public Task Send(IRequest request, CancellationToken cancellationToken = default(CancellationToken))
				{
					return Task.CompletedTask;
				}
			}
		}
	}
}
