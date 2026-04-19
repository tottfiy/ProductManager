using StorageModels.Enums;

namespace Services.DTOs;

public class ProductListDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ProductCategory Category { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public decimal TotalValue { get; set; }
}
