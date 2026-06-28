namespace NordesteFoodAPI.Modules.Products.Domain.DTOs
{
    public record ProductResponseDTO(
        Guid Id,
        string ProductName,
        string ProductDescription,
        decimal ProductPrice,
        bool IsFeatured
    );
}
