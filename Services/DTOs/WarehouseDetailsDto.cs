using StorageModels.Enums;

namespace Services.DTOs;

public class WarehouseDetailsDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public Location Location { get; set; }

    public string LocationName { get; set; } = string.Empty;

    public decimal TotalValue { get; set; }

    public IReadOnlyCollection<ProductListDto> Products { get; set; } = Array.Empty<ProductListDto>();
}
