using StorageModels.Entities;

namespace Repositories;

public interface IStorageRepository
{
    IReadOnlyCollection<WarehouseEntity> GetWarehouses();
    WarehouseEntity? GetWarehouseById(int id);
    IReadOnlyCollection<ProductEntity> GetProducts();
    IReadOnlyCollection<ProductEntity> GetProductsByWarehouse(Guid warehouseGuid);
    ProductEntity? GetProductById(int id);
}
