using StorageModels.Enums;

namespace StorageModels.Entities;

public class WarehouseEntity
{
    public WarehouseEntity()
    {
        Name = string.Empty;
    }

    public WarehouseEntity(int id, string name, Location location)
        : this(Guid.NewGuid(), id, name, location)
    {
    }

    public WarehouseEntity(Guid guid, int id, string name, Location location)
    {
        Guid = guid;
        Id = id;
        Name = name;
        Location = location;
    }

    public Guid Guid { get; set; }

    public int Id { get; set; }

    public string Name { get; set; }

    public Location Location { get; set; }
}
