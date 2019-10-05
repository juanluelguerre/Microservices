using ElGuerre.Microservices.Shipping.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace ElGuerre.Microservices.Shipping.Api.Application.Infrastructure
{
	public class ShippingContext : DbContext
	{
        public ShippingContext() : base()
        {
        }

        public ShippingContext(DbContextOptions<ShippingContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.ApplyConfigurationsFromAssembly(typeof(Startup).Assembly);
		}

		// TODO: Add entities
		// public DbSet<OrderEntity> xxx { get; set; }        
	}
}
