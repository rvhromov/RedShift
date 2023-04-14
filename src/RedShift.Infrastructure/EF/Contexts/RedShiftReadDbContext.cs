using Microsoft.EntityFrameworkCore;
using RedShift.Infrastructure.EF.EntityConfigs;
using RedShift.Infrastructure.EF.ReadModels;

namespace RedShift.Infrastructure.EF.Contexts;

internal sealed class RedShiftReadDbContext : DbContext
{
    public RedShiftReadDbContext(DbContextOptions<RedShiftReadDbContext> options) : base(options)
    {
    }

    public DbSet<UserReadModel> Users { get; set; }
    public DbSet<SkyMarkReadModel> SkyMarks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration<UserReadModel>(new UserConfig());
        modelBuilder.ApplyConfiguration<SkyMarkReadModel>(new SkyMarkConfig());
    }
}
