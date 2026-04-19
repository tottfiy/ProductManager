namespace Services.DTOs;

public class ExistingProductOptionDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string CategoryName { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string SourceWarehouseName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int SourceQuantity { get; set; }

    public string DisplayName { get; set; } = string.Empty;
}
