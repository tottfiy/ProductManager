using Services.DTOs;

namespace Services;

public interface IStorageService
{
    IReadOnlyCollection<WarehouseListDto> GetWarehouseList();
    WarehouseDetailsDto? GetWarehouseDetails(int warehouseId);
    ProductDetailsDto? GetProductDetails(int productId);
}
