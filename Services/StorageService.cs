using Repositories;
using Services.DTOs;
using StorageModels.Entities;

namespace Services;

public class StorageService : IStorageService
{
    private readonly IStorageRepository _storageRepository;

    public StorageService(IStorageRepository storageRepository)
    {
        _storageRepository = storageRepository;
    }

    public async Task<IReadOnlyCollection<WarehouseListDto>> GetWarehouseListAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<WarehouseEntity> warehouses = await _storageRepository.GetWarehousesAsync(cancellationToken);
        IReadOnlyCollection<ProductEntity> products = await _storageRepository.GetProductsAsync(cancellationToken);

        return warehouses
            .Select(warehouse =>
            {
                List<ProductEntity> warehouseProducts = products
                    .Where(product => product.StorageGuid == warehouse.Guid)
                    .ToList();

                return new WarehouseListDto
                {
                    Id = warehouse.Id,
                    Name = warehouse.Name,
                    LocationName = warehouse.Location.ToString(),
                    ProductCount = warehouseProducts.Count,
                    TotalValue = warehouseProducts.Sum(product => product.Price * product.Quantity)
                };
            })
            .ToList();
    }

    public async Task<WarehouseDetailsDto?> GetWarehouseDetailsAsync(int warehouseId, CancellationToken cancellationToken = default)
    {
        WarehouseEntity? warehouse = await _storageRepository.GetWarehouseByIdAsync(warehouseId, cancellationToken);
        if (warehouse is null)
        {
            return null;
        }

        IReadOnlyCollection<ProductEntity> products = await _storageRepository.GetProductsByWarehouseAsync(warehouse.Guid, cancellationToken);
        List<ProductListDto> productDtos = products
            .Select(product => new ProductListDto
            {
                Id = product.Id,
                Name = product.Name,
                Category = product.ProductCategory,
                CategoryName = product.ProductCategory.ToString(),
                Quantity = product.Quantity,
                Price = product.Price,
                TotalValue = product.Price * product.Quantity
            })
            .OrderBy(product => product.Name)
            .ToList();

        return new WarehouseDetailsDto
        {
            Id = warehouse.Id,
            Name = warehouse.Name,
            Location = warehouse.Location,
            LocationName = warehouse.Location.ToString(),
            TotalValue = productDtos.Sum(product => product.TotalValue),
            Products = productDtos
        };
    }

    public async Task<ProductDetailsDto?> GetProductDetailsAsync(int productId, CancellationToken cancellationToken = default)
    {
        ProductEntity? product = await _storageRepository.GetProductByIdAsync(productId, cancellationToken);
        if (product is null)
        {
            return null;
        }

        WarehouseEntity? warehouse = (await _storageRepository.GetWarehousesAsync(cancellationToken))
            .FirstOrDefault(item => item.Guid == product.StorageGuid);

        return new ProductDetailsDto
        {
            Id = product.Id,
            WarehouseId = warehouse?.Id ?? 0,
            WarehouseName = warehouse?.Name ?? "Невідомий склад",
            Name = product.Name,
            Category = product.ProductCategory,
            CategoryName = product.ProductCategory.ToString(),
            Quantity = product.Quantity,
            Price = product.Price,
            TotalValue = product.Price * product.Quantity,
            Description = string.IsNullOrWhiteSpace(product.Description) ? "—" : product.Description
        };
    }

    public async Task<IReadOnlyCollection<ExistingProductOptionDto>> GetExistingProductOptionsAsync(int warehouseId, CancellationToken cancellationToken = default)
    {
        WarehouseEntity? warehouse = await _storageRepository.GetWarehouseByIdAsync(warehouseId, cancellationToken);
        if (warehouse is null)
        {
            return Array.Empty<ExistingProductOptionDto>();
        }

        IReadOnlyCollection<WarehouseEntity> warehouses = await _storageRepository.GetWarehousesAsync(cancellationToken);
        IReadOnlyCollection<ProductEntity> products = await _storageRepository.GetProductsAsync(cancellationToken);

        Dictionary<Guid, string> warehouseNames = warehouses.ToDictionary(item => item.Guid, item => item.Name);

        return products
            .Where(product => product.StorageGuid != warehouse.Guid)
            .OrderBy(product => product.Name)
            .ThenBy(product => warehouseNames.TryGetValue(product.StorageGuid, out string? sourceWarehouseName) ? sourceWarehouseName : string.Empty)
            .Select(product => new ExistingProductOptionDto
            {
                Id = product.Id,
                Name = product.Name,
                CategoryName = product.ProductCategory.ToString(),
                Price = product.Price,
                SourceWarehouseName = warehouseNames.TryGetValue(product.StorageGuid, out string? sourceWarehouseName)
                    ? sourceWarehouseName
                    : "Невідомий склад",
                Description = string.IsNullOrWhiteSpace(product.Description) ? "—" : product.Description,
                SourceQuantity = product.Quantity,
                DisplayName = $"#{product.Id} · {product.Name} · {product.ProductCategory} · {product.Price:C2} · {GetWarehouseNameForDisplay(warehouseNames, product.StorageGuid)}"
            })
            .ToList();
    }

    public async Task<ProductDetailsDto?> AddExistingProductToWarehouseAsync(int warehouseId, int sourceProductId, int quantity, CancellationToken cancellationToken = default)
    {
        if (quantity <= 0)
        {
            throw new InvalidOperationException("Кількість для додавання існуючого товару повинна бути більшою за нуль.");
        }

        ProductEntity? product = await _storageRepository.AddExistingProductToWarehouseAsync(
            warehouseId,
            sourceProductId,
            quantity,
            cancellationToken);

        if (product is null)
        {
            return null;
        }

        return await GetProductDetailsAsync(product.Id, cancellationToken);
    }

    public async Task<string?> GetWarehouseNameAsync(int warehouseId, CancellationToken cancellationToken = default)
    {
        WarehouseEntity? warehouse = await _storageRepository.GetWarehouseByIdAsync(warehouseId, cancellationToken);
        return warehouse?.Name;
    }

    public async Task<WarehouseDetailsDto> SaveWarehouseAsync(WarehouseSaveDto warehouse, CancellationToken cancellationToken = default)
    {
        string name = warehouse.Name.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidOperationException("Назва складу не може бути порожньою.");
        }

        int warehouseId;
        if (warehouse.Id.HasValue)
        {
            bool updated = await _storageRepository.UpdateWarehouseAsync(
                warehouse.Id.Value,
                name,
                warehouse.Location,
                cancellationToken);

            if (!updated)
            {
                throw new InvalidOperationException("Не вдалося оновити склад.");
            }

            warehouseId = warehouse.Id.Value;
        }
        else
        {
            WarehouseEntity createdWarehouse = await _storageRepository.AddWarehouseAsync(name, warehouse.Location, cancellationToken);
            warehouseId = createdWarehouse.Id;
        }

        WarehouseDetailsDto? warehouseDetails = await GetWarehouseDetailsAsync(warehouseId, cancellationToken);
        return warehouseDetails ?? throw new InvalidOperationException("Не вдалося завантажити дані складу після збереження.");
    }

    public Task<bool> DeleteWarehouseAsync(int warehouseId, CancellationToken cancellationToken = default)
    {
        return _storageRepository.DeleteWarehouseAsync(warehouseId, cancellationToken);
    }

    public async Task<ProductDetailsDto?> SaveProductAsync(ProductSaveDto product, CancellationToken cancellationToken = default)
    {
        string name = product.Name.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidOperationException("Назва товару не може бути порожньою.");
        }

        if (product.Quantity < 0)
        {
            throw new InvalidOperationException("Кількість товару не може бути від’ємною.");
        }

        if (product.Price < 0)
        {
            throw new InvalidOperationException("Ціна товару не може бути від’ємною.");
        }

        int productId;
        if (product.Id.HasValue)
        {
            bool updated = await _storageRepository.UpdateProductAsync(
                product.Id.Value,
                name,
                product.Quantity,
                product.Price,
                product.Category,
                product.Description,
                cancellationToken);

            if (!updated)
            {
                throw new InvalidOperationException("Не вдалося оновити товар.");
            }

            productId = product.Id.Value;
        }
        else
        {
            ProductEntity? createdProduct = await _storageRepository.AddProductAsync(
                product.WarehouseId,
                name,
                product.Quantity,
                product.Price,
                product.Category,
                product.Description,
                cancellationToken);

            if (createdProduct is null)
            {
                return null;
            }

            productId = createdProduct.Id;
        }

        return await GetProductDetailsAsync(productId, cancellationToken);
    }

    public Task<bool> DeleteProductAsync(int productId, CancellationToken cancellationToken = default)
    {
        return _storageRepository.DeleteProductAsync(productId, cancellationToken);
    }

    private static string GetWarehouseNameForDisplay(IReadOnlyDictionary<Guid, string> warehouseNames, Guid warehouseGuid)
    {
        return warehouseNames.TryGetValue(warehouseGuid, out string? warehouseName)
            ? warehouseName
            : "Невідомий склад";
    }

}
