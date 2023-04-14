using RedShift.Domain.Enums;

namespace RedShift.Infrastructure.EF.ReadModels;

public abstract class BaseReadModel
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Status Status { get; set; }
}
