namespace ElGuerre.Microservices.Shared
{
	public interface IRepository<T> where T : IAggregateRoot
	{
		IUnitOfWork UnitOfWork { get; }
	}
}
