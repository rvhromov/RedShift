using Microsoft.EntityFrameworkCore;
using RedShift.Domain.Entities;
using RedShift.Infrastructure.EF.EntityConfigs;

namespace RedShift.Infrastructure.EF.Contexts;

internal sealed class RedShiftWriteDbContext : DbContext
{
    public RedShiftWriteDbContext(DbContextOptions<RedShiftWriteDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<SkyMark> SkyMarks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration<User>(new UserConfig());
        modelBuilder.ApplyConfiguration<SkyMark>(new SkyMarkConfig());
    }
}
