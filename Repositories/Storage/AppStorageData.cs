using StorageModels.Entities;

namespace Repositories.Storage;

internal sealed class AppStorageData
{
    public int NextWarehouseId { get; set; }

    public int NextProductId { get; set; }

    public List<WarehouseEntity> Warehouses { get; set; } = new();

    public List<ProductEntity> Products { get; set; } = new();
}
