using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.Infrastructure.Persistence.Configurations;

internal sealed class WalletShareConfiguration : IEntityTypeConfiguration<WalletShare>
{
    public void Configure(EntityTypeBuilder<WalletShare> builder)
    {
        builder.ToTable(nameof(WalletShare));
        builder.HasKey(ws => ws.Id);

        builder
            .HasOne(ws => ws.Wallet)
            .WithMany(w => w.Shares)
            .HasForeignKey(w => w.WalletId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(ws => ws.User)
            .WithMany()
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .Property(ws => ws.Date)
            .IsRequired();

        builder
            .Property(ws => ws.AccessType)
            .IsRequired();

        builder
            .Property(ws => ws.IsAccepted)
            .IsRequired();
    }
}
