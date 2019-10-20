using ElGuerre.Microservices.Ordering.Api.Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Application.Validations
{
	/// <summary>	
	/// Order Validation to be sure its properties have correct values.
	/// </summary>
	/// <seealso cref="https://fluentvalidation.net/start"/>
	public class OrderValidator : AbstractValidator<OrderModel>
	{
		public OrderValidator()
		{
			// RuleSets allow you to group validation rules together which can be executed together as a group whilst ignoring other rules.
			// RuleSet("Order", () =>
			// {
			//		RuleFor(..).NotNull()...;
			// }

			RuleFor(order => order.OrderNumber).NotNull().NotEmpty().GreaterThan(0);
			RuleFor(order => order.Name).NotNull().NotEmpty();

			RuleForEach(order => order.OrderItems).SetValidator(new OrderItemValidator());

		}
	}
}
