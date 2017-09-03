namespace DDD.Domain.Core.SeedWork
{
    public interface IEntity<TKey>
    {
        TKey Id { get; }
    }
}