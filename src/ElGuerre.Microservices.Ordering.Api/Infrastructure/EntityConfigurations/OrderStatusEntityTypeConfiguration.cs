using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Infrastructure.EntityConfigurations
{
	internal class OrderStatusEntityTypeConfiguration
			: IEntityTypeConfiguration<OrderStatus>
	{
		public void Configure(EntityTypeBuilder<OrderStatus> orderStatusConfiguration)
		{
			orderStatusConfiguration.ToTable("orderstatus", OrdersContext.DEFAULT_SCHEMA);

			orderStatusConfiguration.HasKey(o => o.Id);

			orderStatusConfiguration.Property(o => o.Id)
				.HasDefaultValue(1)
				.ValueGeneratedNever()
				.IsRequired();

			orderStatusConfiguration.Property(o => o.Name)
				.HasMaxLength(200)
				.IsRequired();
		}
	}
}
