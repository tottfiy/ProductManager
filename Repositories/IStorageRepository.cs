using StorageModels.Entities;
using StorageModels.Enums;

namespace Repositories;

public interface IStorageRepository
{
    Task<IReadOnlyCollection<WarehouseEntity>> GetWarehousesAsync(CancellationToken cancellationToken = default);
    Task<WarehouseEntity?> GetWarehouseByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ProductEntity>> GetProductsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ProductEntity>> GetProductsByWarehouseAsync(Guid warehouseGuid, CancellationToken cancellationToken = default);
    Task<ProductEntity?> GetProductByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<WarehouseEntity> AddWarehouseAsync(string name, Location location, CancellationToken cancellationToken = default);
    Task<bool> UpdateWarehouseAsync(int id, string name, Location location, CancellationToken cancellationToken = default);
    Task<bool> DeleteWarehouseAsync(int id, CancellationToken cancellationToken = default);
    Task<ProductEntity?> AddProductAsync(
        int warehouseId,
        string name,
        int quantity,
        decimal price,
        ProductCategory productCategory,
        string? description,
        CancellationToken cancellationToken = default);
    Task<ProductEntity?> AddExistingProductToWarehouseAsync(
        int targetWarehouseId,
        int sourceProductId,
        int quantity,
        CancellationToken cancellationToken = default);
    Task<bool> UpdateProductAsync(
        int productId,
        string name,
        int quantity,
        decimal price,
        ProductCategory productCategory,
        string? description,
        CancellationToken cancellationToken = default);
    Task<bool> DeleteProductAsync(int productId, CancellationToken cancellationToken = default);
}
