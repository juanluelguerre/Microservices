using ElGuerre.Microservices.Sales.Api.Application.Models;
using ElGuerre.Microservices.Sales.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ElGuerre.Microservices.Sales.Api.Application.Infrastructure
{
	public class OrdersContext : DbContext
	{
		public DbSet<Domain.Order> Orders { get; set; }
		public virtual DbSet<Domain.Order> Elementos { get; set; }

		public OrdersContext() : base()
        {
        }

        public OrdersContext(DbContextOptions<OrdersContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.ApplyConfigurationsFromAssembly(typeof(Startup).Assembly);
		}

		public class CatalogContextDesignFactory : IDesignTimeDbContextFactory<OrdersContext>
		{
			public OrdersContext CreateDbContext(string[] args)
			{
				IConfigurationRoot configuration = new ConfigurationBuilder()
				   .SetBasePath(Directory.GetCurrentDirectory())
				   .AddUserSecrets<Startup>()
				   .AddJsonFile("appsettings.json")
				   .Build();

				var builder = new DbContextOptionsBuilder<OrdersContext>();

				builder.UseSqlServer(configuration.GetConnectionString("DataBaseConnection"));

				return new OrdersContext(builder.Options);
			}
		}
	}
}
