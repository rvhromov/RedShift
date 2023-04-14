using RedShift.Domain.Abstractions;
using RedShift.Domain.Enums;
using RedShift.Domain.ValueObjects.Users;

namespace RedShift.Domain.Entities;

public class User : Entity
{
    private readonly List<SkyMark> _skyMarks = new();

    private User()
    {
    }

    public string Name { get; private set; }
    public Email Email { get; private set; }
    public string Password { get; private set; }
    public string Salt { get; private set; }
    public RoleType Role { get; private set; }
    public IReadOnlyCollection<SkyMark> SkyMarks => _skyMarks.AsReadOnly();

    public static User Create(string name, string email, string password, string salt)
    {
        return new()
        {
            Name = name,
            Email = email,
            Password = password,
            Salt = salt,
            Role = RoleType.User,
            Status = Status.Active
        };
    }

    public void Update(string name, string email)
    {
        Name = name;
        Email = email;
        SetModificationDate(DateTime.UtcNow);
    }
}
