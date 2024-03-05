using System.Text.Json.Serialization;

namespace Shared.Core.Entity;

public abstract class BaseEntity
{
    [JsonPropertyOrder(0)]
    public Guid Id { get; private set; } = new Guid();
    [JsonPropertyOrder(10)]
    public DateTime CreatedAt { get; protected set; } = DateTime.Now;
    [JsonPropertyOrder(11)]
    public DateTime? UpdatedAt { get; protected  set; }
    [JsonPropertyOrder(12)]
    public DateTime? DeletedAt { get; protected  set; }

    public virtual void Delete() => DeletedAt = DateTime.Now;
    protected virtual void UpdateDate() => UpdatedAt = DateTime.Now;
}