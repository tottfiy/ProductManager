using StorageModels.Enums;

namespace Services.DTOs;

public class WarehouseSaveDto
{
    public int? Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public Location Location { get; set; }
}
