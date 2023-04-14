using RedShift.Domain.ValueObjects.Location;

namespace RedShift.Infrastructure.EF.ReadModels;

public sealed class SkyMarkReadModel : BaseReadModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public Location Location { get; set; }
    public Guid UserId { get; set; }
    public UserReadModel User { get; set; }
}
