using Repositories.Storage;
using StorageModels.Entities;

namespace Repositories;

public class StorageRepository : IStorageRepository
{
    public IReadOnlyCollection<WarehouseEntity> GetWarehouses()
    {
        return FakeStorage.Warehouses.ToList();
    }

    public WarehouseEntity? GetWarehouseById(int id)
    {
        return FakeStorage.Warehouses.FirstOrDefault(warehouse => warehouse.Id == id);
    }

    public IReadOnlyCollection<ProductEntity> GetProducts()
    {
        return FakeStorage.Products.ToList();
    }

    public IReadOnlyCollection<ProductEntity> GetProductsByWarehouse(Guid warehouseGuid)
    {
        return FakeStorage.Products
            .Where(product => product.StorageGuid == warehouseGuid)
            .ToList();
    }

    public ProductEntity? GetProductById(int id)
    {
        return FakeStorage.Products.FirstOrDefault(product => product.Id == id);
    }
}
