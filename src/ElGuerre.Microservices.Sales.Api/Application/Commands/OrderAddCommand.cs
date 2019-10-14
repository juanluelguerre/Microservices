﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api.Application.Commands
{
	public class OrderAddCommand : IRequest
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}
}
