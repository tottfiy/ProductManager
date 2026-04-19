using StorageModels.Enums;

namespace StorageModels.Entities;

public class ProductEntity
{
    public ProductEntity()
    {
        Name = string.Empty;
    }

    public ProductEntity(
        int id,
        string name,
        int quantity,
        Guid storageGuid,
        decimal price,
        ProductCategory productCategory,
        string? description = null)
        : this(Guid.NewGuid(), id, name, quantity, storageGuid, price, productCategory, description)
    {
    }

    public ProductEntity(
        Guid guid,
        int id,
        string name,
        int quantity,
        Guid storageGuid,
        decimal price,
        ProductCategory productCategory,
        string? description = null)
    {
        Guid = guid;
        Id = id;
        Name = name;
        Quantity = quantity;
        StorageGuid = storageGuid;
        Price = price;
        ProductCategory = productCategory;
        Description = description;
    }

    public Guid Guid { get; set; }

    public int Id { get; set; }

    public Guid StorageGuid { get; set; }

    public string Name { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public ProductCategory ProductCategory { get; set; }

    public string? Description { get; set; }
}
