using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NordesteFoodAPI.Modules.Products.Domain.Entities;
using NordesteFoodAPI.Modules.Restaurants.Domain.Entities;
using NordesteFoodAPI.Modules.Stocks.Domain.Entities;
using NordesteFoodAPI.Modules.Stocks.Domain.ValueObjects;

namespace NordesteFoodAPI.Modules.Stocks.Infraestructure.EntitiesConfig
{
    public class StockConfiguration : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(q => q.Quantity)
                .HasConversion(
                    quantity => quantity.Value,
                    value => Quantity.Create(value)
                )
                .HasColumnName("Quantity")
                .IsRequired();

            builder.HasOne<Product>()
                .WithMany()
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Restaurant>()
              .WithMany()
              .HasForeignKey(s => s.RestaurantId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(s => new { s.RestaurantId, s.ProductId})
                .IsUnique();

            builder.Property(s => s.UpdatedAt).IsRequired();
        }
    }
}
