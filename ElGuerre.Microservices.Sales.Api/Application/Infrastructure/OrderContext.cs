using ElGuerre.Microservices.Sales.Api.Application.Models;
using ElGuerre.Microservices.Sales.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace ElGuerre.Microservices.Sales.Api.Application.Infrastructure
{
	public class OrderContext : DbContext
	{
        public OrderContext() : base()
        {
        }

        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.ApplyConfigurationsFromAssembly(typeof(Startup).Assembly);
		}

		public DbSet<OrderEntity> Orders { get; set; }
        public virtual DbSet<OrderEntity> Elementos { get; set; }
	}
}
