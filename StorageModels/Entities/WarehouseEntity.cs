using StorageModels.Enums;

namespace StorageModels.Entities;

public class WarehouseEntity
{
    private readonly Guid _guid;
    private readonly int _id;
    private string _name;
    private Location _location;

    public Guid Guid => _guid;
    public int Id => _id;

    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public Location Location
    {
        get => _location;
        set => _location = value;
    }

    public WarehouseEntity(int id, string name, Location location)
    {
        _guid = Guid.NewGuid();
        _id = id;
        _name = name;
        _location = location;
    }
}
