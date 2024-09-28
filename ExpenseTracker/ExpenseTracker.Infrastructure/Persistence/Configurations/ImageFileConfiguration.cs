using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.Infrastructure.Persistence.Configurations;

public class ImageFileConfiguration : IEntityTypeConfiguration<Domain.Entities.ImageFile>
{
    public void Configure(EntityTypeBuilder<ImageFile> builder)
    {
        builder.ToTable(nameof(ImageFile));
        builder.HasKey(i => i.Id);

        builder
            .HasOne(i => i.Transfer)
            .WithMany(t => t.Images)
            .HasForeignKey(i => i.TransferId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder
            .Property(i => i.Name)
            .HasMaxLength(Constants.DEFAULT_STRING_LENGTH)
            .IsRequired();

        builder
            .Property(i => i.Data)
            .IsRequired();

        builder
            .Property(i => i.Type)
            .HasMaxLength(Constants.DEFAULT_STRING_LENGTH)
            .IsRequired();
    }
}
