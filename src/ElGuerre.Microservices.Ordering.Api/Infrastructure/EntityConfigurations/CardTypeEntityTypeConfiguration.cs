using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Infrastructure.EntityConfigurations
{
	internal class CardTypeEntityTypeConfiguration
		: IEntityTypeConfiguration<CardType>
	{
		public void Configure(EntityTypeBuilder<CardType> cardTypesConfiguration)
		{
			cardTypesConfiguration.ToTable("cardtypes", OrderingContext.DEFAULT_SCHEMA);

			cardTypesConfiguration.HasKey(ct => ct.Id);

			cardTypesConfiguration.Property(ct => ct.Id)
				.HasDefaultValue(1)
				.ValueGeneratedNever()
				.IsRequired();

			cardTypesConfiguration.Property(ct => ct.Name)
				.HasMaxLength(200)
				.IsRequired();
		}
	}
}
