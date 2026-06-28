using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NordesteFoodAPI.Modules.Products.Domain.Entities;
using NordesteFoodAPI.Modules.Restaurants.Domain.Entities;
using NordesteFoodAPI.Modules.UnitProducts.Domain.Entities;
using NordesteFoodAPI.Modules.UnitProducts.Domain.ValueObjects;

namespace NordesteFoodAPI.Modules.UnitProducts.Infraestructure.EntityConfig
{
    public class UnitProductConfiguration : IEntityTypeConfiguration<UnitProduct>
    {
        public void Configure(EntityTypeBuilder<UnitProduct> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.IsAvailable)
                .IsRequired();

            builder.Property(u => u.CreatedAt)
                .IsRequired();

            builder.Property(u => u.UpdatedAt)
               .IsRequired();

            builder.Property(u => u.Price)
                .HasConversion(
                    price => price.Value,
                    value => UnitPrice.Create(value))
                .HasColumnName("Price")
                .IsRequired();

            builder.HasOne<Product>()
                .WithMany()
                .HasForeignKey(u => u.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Restaurant>()
              .WithMany()
              .HasForeignKey(u => u.RestaurantId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(u => new { u.ProductId, u.RestaurantId })
              .IsUnique();
        }
    }
}
