//using ElGuerre.Microservices.Sales.Api.Application.Models;
//using MassTransit;
//using MassTransit.Saga;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ElGuerre.Microservices.Sales.Api.Application.Sagas
//{
//	public class UpdatePolicySaga :ISaga, InitiatedBy<UpdateOrder>
//	{
//		public Guid CorrelationId { get; set; }

//		public async Task Consume(ConsumeContext<UpdateOrder> context)
//		{
//			await Console.Out.WriteLineAsync($"Processing policy number {context.Message.OrderId} in saga.");
//		}
//	}
//}
