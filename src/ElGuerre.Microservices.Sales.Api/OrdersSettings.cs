﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api
{
	public class OrdersSettings
	{
		public string EventBusConnection { get; set; }
		public string OrdersDBConnection { get; set; }
	}
}
