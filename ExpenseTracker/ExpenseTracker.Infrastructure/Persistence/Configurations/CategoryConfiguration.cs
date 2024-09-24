using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.Infrastructure.Persistence.Configurations
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");

            builder.HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .IsRequired();

            builder.HasMany(c => c.Transfers)
                .WithOne(t => t.Category)
                .HasForeignKey(x => x.CategoryId);

            builder.Property(x => x.Name)
                .HasMaxLength(Constants.DEFAULT_STRING_LENGTH)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(Constants.MAX_STRING_LENGTH)
                .IsRequired(false);
        }
    }
}
