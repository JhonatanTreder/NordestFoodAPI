using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NordesteFoodAPI.Modules.Products.Domain.Entities;
using NordesteFoodAPI.Modules.Products.Domain.ValueObjects;

namespace NordesteFoodAPI.Modules.Products.Infraestructure.EntityConfig
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.ProductName)
                .HasConversion(
                    name => name.Value,
                    value => ProductName.Create(value)
                )
                .HasColumnName("ProductName")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.ProductDescription)
                .HasConversion(
                    description => description.Value,
                    value => ProductDescription.Create(value)
                )
                .HasColumnName("ProductDescription")
                .HasMaxLength(270);

            builder.Property(p => p.ProductPrice)
                .HasConversion(
                    price => price.Value,
                    value => Price.Create(value)
                )
                .HasColumnName("UnitPrice")
                .IsRequired();

            builder.Property(p => p.IsFeatured)
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .IsRequired();
        }
    }
}
