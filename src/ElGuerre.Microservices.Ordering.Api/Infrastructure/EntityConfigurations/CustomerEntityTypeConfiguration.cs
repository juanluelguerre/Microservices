using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Infrastructure.EntityConfigurations
{
	internal class CustomerEntityTypeConfiguration
		: IEntityTypeConfiguration<Customer>
	{
		public void Configure(EntityTypeBuilder<Customer> buyerConfiguration)
		{
			buyerConfiguration.ToTable("customers", OrdersContext.DEFAULT_SCHEMA);

			buyerConfiguration.HasKey(b => b.Id);

			buyerConfiguration.Ignore(b => b.DomainEvents);

			buyerConfiguration.Property(b => b.Id)
				.ForSqlServerUseSequenceHiLo("customerseq", OrdersContext.DEFAULT_SCHEMA);

			buyerConfiguration.Property(b => b.Identity)
				.HasMaxLength(200)
				.IsRequired();

			buyerConfiguration.HasIndex("Identity")
			  .IsUnique(true);

			buyerConfiguration.Property(b => b.Name);

			buyerConfiguration.HasMany(b => b.PaymentMethods)
			   .WithOne()
			   .HasForeignKey("CustomerId")
			   .OnDelete(DeleteBehavior.Cascade);

			var navigation = buyerConfiguration.Metadata.FindNavigation(nameof(Customer.PaymentMethods));

			navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
