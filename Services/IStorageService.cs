
using StorageModels.Entities;
using System.Collections.Generic;

namespace Services
{
    public interface IStorageService
    {
        IEnumerable<WarehouseEntity> GetWarehouses();
        WarehouseEntity GetWarehouseById(int id);
        IEnumerable<ProductEntity> GetProducts();
        ProductEntity GetProductById(int id);
    }
}
