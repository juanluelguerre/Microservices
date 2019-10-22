using ElGuerre.Microservices.Ordering.Api.Application.Models;
using ElGuerre.Microservices.Ordering.Api.Application.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Application.Queries
{
	public class CustomerByUserIdQuery : IRequest<CustomerModel>
	{
		public string UserId { get; private set; }

		public CustomerByUserIdQuery(string userId)
		{
			UserId = userId;
		}
	}
}
