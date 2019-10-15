using ElGuerre.Microservices.Billing.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace ElGuerre.Microservices.Billing.Api.Infrastructure
{
	public class BillingContext : DbContext
	{
        public BillingContext() : base()
        {
        }

        public BillingContext(DbContextOptions<BillingContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.ApplyConfigurationsFromAssembly(typeof(Startup).Assembly);
		}

		public DbSet<OrderEntity> Orders { get; set; }        
	}
}
