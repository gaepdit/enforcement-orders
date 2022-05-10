namespace Enfo.Domain.BaseEntities;

/// <summary>
/// The default implementation of <see cref="IdentifiedEntity{TKey}"/> which uses an int as the primary key.
/// </summary>
public abstract class IdentifiedEntity : IdentifiedEntity<int> { }

/// <summary>
/// Represents an entity with an identifier property
/// </summary>
/// <typeparam name="TKey">The type used for the primary key for the entity.</typeparam>
public abstract class IdentifiedEntity<TKey> where TKey : IEquatable<TKey>
{
    public TKey Id { get; init; }
}
