using ElGuerre.Microservices.Sales.Api.Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api.Application.Validations
{
	/// <summary>	
	/// Order Validation to be sure its properties have correct values.
	/// </summary>
	/// <seealso cref="https://fluentvalidation.net/start"/>
	public class OrderValidator : AbstractValidator<Order>
	{
		public OrderValidator()
		{
			RuleFor(order => order.Name).NotNull().NotEmpty();
		}
	}
}
