using StorageModels.Enums;

namespace StorageModels.Entities;

public class ProductEntity
{
    private readonly Guid _guid;
    private int _id;
    private Guid _storageGuid;
    private string _name;
    private int _quantity;
    private decimal _price;
    private ProductCategory _productCategory;
    private string? _description;

    public Guid Guid => _guid;

    public int Id
    {
        get => _id;
        private set => _id = value;
    }

    public Guid StorageGuid
    {
        get => _storageGuid;
        private set => _storageGuid = value;
    }

    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public int Quantity
    {
        get => _quantity;
        set => _quantity = value;
    }

    public decimal Price
    {
        get => _price;
        set => _price = value;
    }

    public ProductCategory ProductCategory
    {
        get => _productCategory;
        set => _productCategory = value;
    }

    public string? Description
    {
        get => _description;
        set => _description = value;
    }

    public ProductEntity(
        int id,
        string name,
        int quantity,
        Guid storageGuid,
        decimal price,
        ProductCategory productCategory,
        string? description = null)
    {
        _guid = Guid.NewGuid();
        _id = id;
        _storageGuid = storageGuid;
        _name = name;
        _quantity = quantity;
        _price = price;
        _productCategory = productCategory;
        _description = description;
    }
}
