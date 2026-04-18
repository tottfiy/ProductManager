using System.Text.Json;
using System.Text.Json.Serialization;
using Repositories.Storage;
using StorageModels.Entities;
using StorageModels.Enums;

namespace Repositories;

public class StorageRepository : IStorageRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly SemaphoreSlim _syncLock = new(1, 1);
    private readonly string _storageFilePath;

    public StorageRepository()
    {
        string directoryPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "ProductManagerLab4");

        Directory.CreateDirectory(directoryPath);
        _storageFilePath = Path.Combine(directoryPath, "storage.json");
    }

    public async Task<IReadOnlyCollection<WarehouseEntity>> GetWarehousesAsync(CancellationToken cancellationToken = default)
    {

    }

    public async Task<WarehouseEntity?> GetWarehouseByIdAsync(int id, CancellationToken cancellationToken = default)
    {

    }

    public async Task<IReadOnlyCollection<ProductEntity>> GetProductsAsync(CancellationToken cancellationToken = default)
    {

    }

    public async Task<IReadOnlyCollection<ProductEntity>> GetProductsByWarehouseAsync(Guid warehouseGuid, CancellationToken cancellationToken = default)
    {

    }

    public async Task<ProductEntity?> GetProductByIdAsync(int id, CancellationToken cancellationToken = default)
    {

    }

    public async Task<WarehouseEntity> AddWarehouseAsync(string name, Location location, CancellationToken cancellationToken = default)
    {

    }

    public async Task<bool> UpdateWarehouseAsync(int id, string name, Location location, CancellationToken cancellationToken = default)
    {
        await _syncLock.WaitAsync(cancellationToken);
        try
        {
            AppStorageData data = await ReadDataInternalAsync(cancellationToken);
            WarehouseEntity? warehouse = data.Warehouses.FirstOrDefault(item => item.Id == id);
            if (warehouse is null)
            {
                return false;
            }

            warehouse.Name = name;
            warehouse.Location = location;
            await SaveDataInternalAsync(data, cancellationToken);

            return true;
        }
        finally
        {
            _syncLock.Release();
        }
    }

    public async Task<bool> DeleteWarehouseAsync(int id, CancellationToken cancellationToken = default)
    {
        await _syncLock.WaitAsync(cancellationToken);
        try
        {
            AppStorageData data = await ReadDataInternalAsync(cancellationToken);
            WarehouseEntity? warehouse = data.Warehouses.FirstOrDefault(item => item.Id == id);
            if (warehouse is null)
            {
                return false;
            }

            data.Warehouses.Remove(warehouse);
            data.Products.RemoveAll(product => product.StorageGuid == warehouse.Guid);
            await SaveDataInternalAsync(data, cancellationToken);

            return true;
        }
        finally
        {
            _syncLock.Release();
        }
    }

    public async Task<ProductEntity?> AddProductAsync(
        int warehouseId,
        string name,
        int quantity,
        decimal price,
        ProductCategory productCategory,
        string? description,
        CancellationToken cancellationToken = default)
    {

    }

    public async Task<bool> UpdateProductAsync(
        int productId,
        string name,
        int quantity,
        decimal price,
        ProductCategory productCategory,
        string? description,
        CancellationToken cancellationToken = default)
    {
        await _syncLock.WaitAsync(cancellationToken);
        try
        {
            AppStorageData data = await ReadDataInternalAsync(cancellationToken);
            ProductEntity? product = data.Products.FirstOrDefault(item => item.Id == productId);
            if (product is null)
            {
                return false;
            }

            product.Name = name;
            product.Quantity = quantity;
            product.Price = price;
            product.ProductCategory = productCategory;
            product.Description = description;
            await SaveDataInternalAsync(data, cancellationToken);

            return true;
        }
        finally
        {
            _syncLock.Release();
        }
    }

    public async Task<bool> DeleteProductAsync(int productId, CancellationToken cancellationToken = default)
    {
        await _syncLock.WaitAsync(cancellationToken);
        try
        {
            AppStorageData data = await ReadDataInternalAsync(cancellationToken);
            ProductEntity? product = data.Products.FirstOrDefault(item => item.Id == productId);
            if (product is null)
            {
                return false;
            }

            data.Products.Remove(product);
            await SaveDataInternalAsync(data, cancellationToken);

            return true;
        }
        finally
        {
            _syncLock.Release();
        }
    }

    private async Task<AppStorageData> ReadDataInternalAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(_storageFilePath))
        {
            AppStorageData initialData = CreateInitialData();
            await SaveDataInternalAsync(initialData, cancellationToken);
            return initialData;
        }

        string json = await File.ReadAllTextAsync(_storageFilePath, cancellationToken);
        if (string.IsNullOrWhiteSpace(json))
        {
            AppStorageData initialData = CreateInitialData();
            await SaveDataInternalAsync(initialData, cancellationToken);
            return initialData;
        }

        AppStorageData? data = JsonSerializer.Deserialize<AppStorageData>(json, JsonOptions);
        data ??= CreateInitialData();
        EnsureCounters(data);

        return data;
    }

    private async Task SaveDataInternalAsync(AppStorageData data, CancellationToken cancellationToken)
    {
        EnsureCounters(data);
        string json = JsonSerializer.Serialize(data, JsonOptions);
        await File.WriteAllTextAsync(_storageFilePath, json, cancellationToken);
    }

    private static void EnsureCounters(AppStorageData data)
    {
        int nextWarehouseId = data.Warehouses.Count == 0
            ? 1
            : data.Warehouses.Max(warehouse => warehouse.Id) + 1;

        int nextProductId = data.Products.Count == 0
            ? 1
            : data.Products.Max(product => product.Id) + 1;

        if (data.NextWarehouseId < nextWarehouseId)
        {
            data.NextWarehouseId = nextWarehouseId;
        }

        if (data.NextProductId < nextProductId)
        {
            data.NextProductId = nextProductId;
        }
    }

    private static AppStorageData CreateInitialData()
    {
        WarehouseEntity warehouse1 = new(1, "Волинська Промбаза", Location.Kyiv);
        WarehouseEntity warehouse2 = new(2, "Склад Львівських круасанів", Location.Lviv);
        WarehouseEntity warehouse3 = new(3, "Офісний склад", Location.Dnipro);

        return new AppStorageData
        {
            NextWarehouseId = 4,
            NextProductId = 13,
            Warehouses =
            [
                warehouse1,
                warehouse2,
                warehouse3
            ],
            Products =
            [
                new ProductEntity(1, "Laptop Dell XPS", 5, warehouse1.Guid, 45000m, ProductCategory.Electronics, "High-end ultrabook"),
                new ProductEntity(2, "iPhone 15", 8, warehouse1.Guid, 52000m, ProductCategory.Electronics, "Apple smartphone"),
                new ProductEntity(3, "Булочка з корицею", 400, warehouse1.Guid, 24.99m, ProductCategory.Food, "З Сільпо"),
                new ProductEntity(4, "Рево Жовте", 105, warehouse1.Guid, 56m, ProductCategory.Food, "Подільська смаковинка"),
                new ProductEntity(5, "Desk Table", 10, warehouse1.Guid, 7000m, ProductCategory.Furniture, "Wooden office desk"),
                new ProductEntity(6, "Wireless Mouse", 25, warehouse1.Guid, 900m, ProductCategory.Electronics, "Logitech mouse"),
                new ProductEntity(7, "Keyboard Mechanical", 12, warehouse1.Guid, 2500m, ProductCategory.Electronics, "RGB keyboard"),
                new ProductEntity(8, "Барабанні палички Vater", 6, warehouse1.Guid, 610m, ProductCategory.Tools, "Мої улюблені"),
                new ProductEntity(9, "Winter Jacket", 20, warehouse1.Guid, 2800m, ProductCategory.Clothing, "Men jacket"),
                new ProductEntity(10, "Sneakers Nike", 18, warehouse1.Guid, 3200m, ProductCategory.Clothing, "Sport shoes"),
                new ProductEntity(11, "Круасан", 500, warehouse2.Guid, 45m, ProductCategory.Food, "Пухкенькі"),
                new ProductEntity(12, "Салат айсберг", 100, warehouse2.Guid, 25.5m, ProductCategory.Food, "Iceberg")
            ]
        };
    }
}
