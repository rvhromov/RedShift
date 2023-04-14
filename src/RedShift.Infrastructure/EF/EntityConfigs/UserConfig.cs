using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RedShift.Domain.Entities;
using RedShift.Domain.ValueObjects.Users;
using RedShift.Infrastructure.EF.DataSeeds;
using RedShift.Infrastructure.EF.ReadModels;

namespace RedShift.Infrastructure.EF.EntityConfigs;

internal sealed class UserConfig : IEntityTypeConfiguration<User>, IEntityTypeConfiguration<UserReadModel>
{
    private const string UserTableName = "Users";

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(UserTableName);
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder
            .Property(x => x.CreatedAt)
            .IsRequired();

        builder
            .Property(x => x.Name)
            .IsRequired();

        builder
            .Property(x => x.Email)
            .HasConversion(new ValueConverter<Email, string>(e => e.Value, e => new Email(e)))
            .IsRequired();

        builder
            .Property(x => x.Password)
            .IsRequired();

        builder
            .Property(x => x.Salt)
            .IsRequired();

        builder
            .Property(x => x.Role)
            .IsRequired();

        builder
            .HasMany(x => x.SkyMarks)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public void Configure(EntityTypeBuilder<UserReadModel> builder)
    {
        builder.ToTable(UserTableName);
        builder.HasKey(x => x.Id);

        builder
            .HasMany(x => x.SkyMarks)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(UserSeed.GetData());
    }
}
