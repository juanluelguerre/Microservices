using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Infrastructure.EntityConfigurations
{
	internal class OrderItemEntityTypeConfiguration
		: IEntityTypeConfiguration<OrderItem>
	{
		public void Configure(EntityTypeBuilder<OrderItem> orderItemConfiguration)
		{
			orderItemConfiguration.ToTable("orderItems", OrderingContext.DEFAULT_SCHEMA);

			orderItemConfiguration.HasKey(o => o.Id);

			orderItemConfiguration.Ignore(b => b.DomainEvents);

			orderItemConfiguration.Property(o => o.Id)
				.ForSqlServerUseSequenceHiLo("orderitemseq");

			orderItemConfiguration.Property<int>("OrderId")
				.IsRequired();

			orderItemConfiguration.Property<decimal>("Discount")
				.IsRequired();

			orderItemConfiguration.Property<int>("ProductId")
				.IsRequired();

			orderItemConfiguration.Property<string>("ProductName")
				.IsRequired();

			orderItemConfiguration.Property<decimal>("UnitPrice")
				.IsRequired();

			orderItemConfiguration.Property<int>("Units")
				.IsRequired();
		}
	}
}
