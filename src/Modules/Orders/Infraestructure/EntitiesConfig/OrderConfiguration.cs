using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NordesteFoodAPI.Modules.Orders.Domain.Entities;
using NordesteFoodAPI.Modules.Orders.Domain.ValueObjects;
using NordesteFoodAPI.Modules.Restaurants.Domain.Entities;
using NordesteFoodAPI.Shared.Infraestructure.Identity;

namespace NordesteFoodAPI.Modules.Orders.Infraestructure.EntitiesConfig
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.OrderStatus)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(o => o.OrderChannel)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(o => o.Total).IsRequired();

            builder.Property(o => o.UpdatedAt).IsRequired();
            builder.Property(o => o.RequestedAt).IsRequired();

            builder.HasOne<Restaurant>()
                .WithMany()
                .HasForeignKey(o => o.RestaurantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.OwnsMany(i => i.Items, item =>
            {
                item.WithOwner().HasForeignKey(i => i.OrderId);

                item.HasKey(i => i.Id);

                item.Property(i => i.Quantity)
                    .HasConversion(q => q.Value, value => Quantity.Create(value))
                    .IsRequired();

                item.Property(i => i.ProductId).IsRequired();

                item.Property(i => i.Subtotal).IsRequired();
                item.Property(i => i.UnitPrice).IsRequired();

                item.ToTable("OrderItems");
            });
        }
    }
}
