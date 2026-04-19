using Services.DTOs;

namespace Services;

public interface IStorageService
{
    Task<IReadOnlyCollection<WarehouseListDto>> GetWarehouseListAsync(CancellationToken cancellationToken = default);
    Task<WarehouseDetailsDto?> GetWarehouseDetailsAsync(int warehouseId, CancellationToken cancellationToken = default);
    Task<ProductDetailsDto?> GetProductDetailsAsync(int productId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ExistingProductOptionDto>> GetExistingProductOptionsAsync(int warehouseId, CancellationToken cancellationToken = default);
    Task<ProductDetailsDto?> AddExistingProductToWarehouseAsync(int warehouseId, int sourceProductId, int quantity, CancellationToken cancellationToken = default);
    Task<string?> GetWarehouseNameAsync(int warehouseId, CancellationToken cancellationToken = default);
    Task<WarehouseDetailsDto> SaveWarehouseAsync(WarehouseSaveDto warehouse, CancellationToken cancellationToken = default);
    Task<bool> DeleteWarehouseAsync(int warehouseId, CancellationToken cancellationToken = default);
    Task<ProductDetailsDto?> SaveProductAsync(ProductSaveDto product, CancellationToken cancellationToken = default);
    Task<bool> DeleteProductAsync(int productId, CancellationToken cancellationToken = default);
}
