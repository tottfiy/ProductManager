namespace Services.DTOs;

public class WarehouseListDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string LocationName { get; set; } = string.Empty;

    public int ProductCount { get; set; }

    public decimal TotalValue { get; set; }
}
