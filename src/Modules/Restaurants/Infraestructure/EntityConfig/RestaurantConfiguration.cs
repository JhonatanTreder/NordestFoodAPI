using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NordesteFoodAPI.Modules.Restaurants.Domain.Entities;

namespace NordesteFoodAPI.Modules.Restaurants.Infraestructure.EntityConfig
{
    public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.IsActive);
            builder.Property(r => r.CreatedAt);

            builder.OwnsOne(r => r.Address, address =>
            {
                address.Property(a => a.Value)
                    .HasColumnName("Address");
            });

            builder.OwnsOne(c => c.ComercialName, comercialName =>
            {
                comercialName.Property(c => c.Value)
                    .HasColumnName("ComercialName")
                    .HasMaxLength(35);
            });

            builder.OwnsOne(r => r.OperationTime, operationTime =>
            {
                operationTime.Property(o => o.OpeningTime)
                    .HasColumnName("OpeningTime");

                operationTime.Property(c => c.ClosingTime)
                    .HasColumnName("ClosingTime");
            });
        }
    }
}
