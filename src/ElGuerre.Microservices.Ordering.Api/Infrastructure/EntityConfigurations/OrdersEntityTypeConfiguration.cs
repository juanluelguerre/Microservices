using ElGuerre.Microservices.Ordering.Api.Domain;
using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Customers;
using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ElGuerre.Microservices.Ordering.Api.Infrastructure.EntityConfigurations
{
	internal class OrdersEntityTypeConfiguration
		: IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			if (builder is null)
			{
				throw new System.ArgumentNullException(nameof(builder));
			}
			//builder.ToTable("Orders");

			//builder.HasKey(ci => ci.Id);

			//builder.Property(ci => ci.Id)			   
			//   .ForSqlServerUseSequenceHiLo("orders_brand_hilo")
			//   .IsRequired();

			//builder.Property(cb => cb.Name)
			//	.IsRequired()
			//	.HasMaxLength(100);


			builder.ToTable("orders", OrdersContext.DEFAULT_SCHEMA);

			builder.HasKey(o => o.Id);

			builder.Ignore(b => b.DomainEvents);

			builder.Property(o => o.Id)
				.ForSqlServerUseSequenceHiLo("orderseq", OrdersContext.DEFAULT_SCHEMA);

			//Address value object persisted as owned entity type supported since EF Core 2.0
			builder.OwnsOne(o => o.Address);

			builder.Property<DateTime>("OrderDate").IsRequired();
			builder.Property<int?>("CustomerId").IsRequired(false);
			builder.Property<int>("OrderStatusId").IsRequired();
			builder.Property<int?>("PaymentMethodId").IsRequired(false);
			builder.Property<string>("Description").IsRequired(false);

			var navigation = builder.Metadata.FindNavigation(nameof(Order.OrderItems));

			// DDD Patterns comment:
			//Set as field (New since EF 1.1) to access the OrderItem collection property through its field
			navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			builder.HasOne<PaymentMethod>()
				.WithMany()
				.HasForeignKey("PaymentMethodId")
				.IsRequired(false)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne<Customer>()
				.WithMany()
				.IsRequired(false)
				.HasForeignKey("CustomerId");

			builder.HasOne(o => o.OrderStatus)
				.WithMany()
				.HasForeignKey("OrderStatusId");

		}
	}
}
