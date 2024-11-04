using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.Infrastructure.Persistence.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasOne(u => u.Image)
            .WithOne()
            .HasForeignKey<ApplicationUser>(u => u.ImageFileId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .Navigation(t => t.Image)
            .AutoInclude();

        builder.Property(u => u.FirstName)
            .HasMaxLength(Constants.DEFAULT_STRING_LENGTH);

        builder.Property(u => u.LastName)
            .HasMaxLength(Constants.DEFAULT_STRING_LENGTH);
    }
}