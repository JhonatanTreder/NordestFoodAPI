using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NordesteFoodAPI.Modules.Feedbacks.Domain.Entities;
using NordesteFoodAPI.Modules.Orders.Domain.Entities;

namespace NordesteFoodAPI.Modules.Feedbacks.Infreaestructure.EntitiesConfig
{
    public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.HasKey(f => f.Id);

            builder.HasOne(f => f.Order)
                .WithOne()
                .HasForeignKey<Feedback>(f => f.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(f => f.Comment)
                .HasColumnName("Comment");

            builder.Property(f => f.CreatedAt);

            builder.Property(f => f.Satisfaction)
                .HasConversion<string>()
                .HasColumnName("Satisfaction");
        }
    }
}
