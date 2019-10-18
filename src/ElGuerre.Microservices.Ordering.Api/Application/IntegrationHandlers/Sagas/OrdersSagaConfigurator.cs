using GreenPipes;
using MassTransit;
using MassTransit.SagaConfigurators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Application.IntegrationHandlers.Sagas
{
	public class OrdersSagaConfigurator : ISagaConfigurator<OrdersStateMachine>
	{
		public void AddPipeSpecification(IPipeSpecification<SagaConsumeContext<OrdersStateMachine>> specification)
		{
			throw new NotImplementedException();
		}

		public void ConfigureMessage<T>(Action<ISagaMessageConfigurator<T>> configure) where T : class
		{
			throw new NotImplementedException();
		}

		public ConnectHandle ConnectSagaConfigurationObserver(ISagaConfigurationObserver observer)
		{
			throw new NotImplementedException();
		}

		public void Message<T>(Action<ISagaMessageConfigurator<T>> configure) where T : class
		{
			throw new NotImplementedException();
		}

		public void SagaMessage<T>(Action<ISagaMessageConfigurator<OrdersStateMachine, T>> configure) where T : class
		{
			throw new NotImplementedException();
		}
	}
}
