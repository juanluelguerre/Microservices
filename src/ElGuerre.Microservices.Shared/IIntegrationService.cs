using ElGuerre.Microservices.Messages;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Shared.Infrastructure
{
	public interface IIntegrationService
	{
		Task PublishToEventBusAsync<T>(T message, string appName) where T : IEvent;
		Task<Q> SendToEventBusAsync<T, Q>(T command, string appName) where T : class, ICommand<Q> where Q : class;
	}
}
