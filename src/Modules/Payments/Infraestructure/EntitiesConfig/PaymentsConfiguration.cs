using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NordesteFoodAPI.Modules.Orders.Domain.Entities;
using NordesteFoodAPI.Modules.Payments.Domain.Entities;

namespace NordesteFoodAPI.Modules.Payments.Infraestructure.EntitiesConfig
{
    public class PaymentsConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.PaymentStatus)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(p => p.PaymentMethod)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(p => p.PaymentProvider)
                .HasConversion<string>()
                .IsRequired();

            builder.HasOne<Order>()
                .WithMany()
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(p => p.TransactionRef)
                .IsUnique();
        }
    }
}
