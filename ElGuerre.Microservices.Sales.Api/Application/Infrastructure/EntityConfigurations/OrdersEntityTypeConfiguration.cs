using ElGuerre.Microservices.Sales.Api.Application.Models;
using ElGuerre.Microservices.Sales.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElGuerre.Microservices.Sales.Api.Application.Infrastructure.EntityConfigurations
{
	internal class OrdersEntityTypeConfiguration
		: IEntityTypeConfiguration<OrderEntity>
	{
		public void Configure(EntityTypeBuilder<OrderEntity> builder)
		{
			builder.ToTable("Orders");

			builder.HasKey(ci => ci.Id);

			builder.Property(ci => ci.Id)			   
			   .ForSqlServerUseSequenceHiLo("orders_brand_hilo")
			   .IsRequired();

			builder.Property(cb => cb.Name)
				.IsRequired()
				.HasMaxLength(100);
		}
	}
}
