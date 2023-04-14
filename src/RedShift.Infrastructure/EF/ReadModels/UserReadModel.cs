using RedShift.Domain.Enums;

namespace RedShift.Infrastructure.EF.ReadModels;

public sealed class UserReadModel : BaseReadModel
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public RoleType Role { get; set; }
    public ICollection<SkyMarkReadModel> SkyMarks { get; set; }
}
