using NordesteFoodAPI.Modules.Products.Domain.DTOs;
using NordesteFoodAPI.Modules.Products.Domain.Entities;
using NordesteFoodAPI.Shared.Common.Results;

namespace NordesteFoodAPI.Modules.Products.Domain.Contracts
{
    public interface IProductsRepository
    {
        Task<Product?> FindByIdAsync(Guid id);
        Task<Result<ProductResponseDTO>> CreateAsync(Product productRequestDTO);
        Task<Result<IEnumerable<Product>>> FindByIdsAsync(IEnumerable<Guid> ids);
    }
}
