using StorageModels.Enums;

namespace Services.DTOs;

public class ProductSaveDto
{
    public int? Id { get; set; }

    public int WarehouseId { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public ProductCategory Category { get; set; }

    public string? Description { get; set; }
}
