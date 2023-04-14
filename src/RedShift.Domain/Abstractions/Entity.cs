using RedShift.Domain.Enums;

namespace RedShift.Domain.Abstractions;

public abstract class Entity
{
    protected Entity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; protected set; }
    public Status Status { get; protected set; }

    protected void SetModificationDate(DateTime utcDate) => 
        UpdatedAt = utcDate;
}