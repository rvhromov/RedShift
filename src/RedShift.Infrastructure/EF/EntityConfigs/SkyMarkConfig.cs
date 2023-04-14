using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RedShift.Domain.Entities;
using RedShift.Infrastructure.EF.ReadModels;

namespace RedShift.Infrastructure.EF.EntityConfigs;

internal sealed class SkyMarkConfig : IEntityTypeConfiguration<SkyMark>, IEntityTypeConfiguration<SkyMarkReadModel>
{
    private const string SkyMarkTableName = "SkyMarks";

    public void Configure(EntityTypeBuilder<SkyMark> builder)
    {
        builder.ToTable(SkyMarkTableName);
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder
            .Property(x => x.CreatedAt)
            .IsRequired();

        builder
            .Property(x => x.Title)
            .IsRequired();

        builder.OwnsOne(x => x.Location, n =>
        {
            n.Property(p => p.Declination).HasColumnName("Declination").IsRequired();
            n.Property(p => p.RightAscension).HasColumnName("RightAscension").IsRequired();
        });

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.SkyMarks)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }

    public void Configure(EntityTypeBuilder<SkyMarkReadModel> builder)
    {
        builder.ToTable(SkyMarkTableName);
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Location, n =>
        {
            n.Property(p => p.Declination).HasColumnName("Declination").IsRequired();
            n.Property(p => p.RightAscension).HasColumnName("RightAscension").IsRequired();
        });

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.SkyMarks)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
