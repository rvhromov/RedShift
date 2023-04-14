using RedShift.Domain.Abstractions;
using RedShift.Domain.Enums;
using RedShift.Domain.ValueObjects.Location;

namespace RedShift.Domain.Entities;

public class SkyMark : Entity
{
    private SkyMark()
    {
    }

    public string Title { get; private set; }
    public string Description { get; private set; }
    public string Image { get; private set; }
    public Location Location { get; private set; }

    public Guid UserId { get; private set; }
    public virtual User User { get; private set; }

    public static SkyMark Create(string title, string description, string image, Location location, User user)
    {
        return new()
        {
            Title = title,
            Description = description,
            Image = image,
            Location = location,
            UserId = user.Id,
            Status = Status.Active
        };
    }

    public void Update(string title, string description, Location location)
    {
        Title = title;
        Description = description;
        Location = location;
        SetModificationDate(DateTime.UtcNow);
    }
}
