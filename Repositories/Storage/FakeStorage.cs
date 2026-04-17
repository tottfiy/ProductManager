using StorageModels.Entities;
using StorageModels.Enums;

namespace Repositories.Storage;

internal static class FakeStorage
{
    internal static List<WarehouseEntity> Warehouses { get; }
    internal static List<ProductEntity> Products { get; }

    static FakeStorage()
    {
        var warehouse1 = new WarehouseEntity(
            id: 1,
            name: "Волинська Промбаза",
            location: Location.Kyiv);

        var warehouse2 = new WarehouseEntity(
            id: 2,
            name: "Склад Львівських круасанів",
            location: Location.Lviv);

        var warehouse3 = new WarehouseEntity(
            id: 3,
            name: "Офісний склад",
            location: Location.Dnipro);

        Warehouses =
        [
            warehouse1,
            warehouse2,
            warehouse3
        ];

        Products =
        [
            new ProductEntity(1, "Laptop Dell XPS", 5, warehouse1.Guid, 45000m, StorageModels.Enums.ProductCategory.Electronics, "High-end ultrabook"),
            new ProductEntity(2, "iPhone 15", 8, warehouse1.Guid, 52000m, StorageModels.Enums.ProductCategory.Electronics, "Apple smartphone"),
            new ProductEntity(3, "Булочка з корицею", 400, warehouse1.Guid, 24.99m, StorageModels.Enums.ProductCategory.Food, "З Сільпо"),
            new ProductEntity(4, "Рево Жовте", 105, warehouse1.Guid, 56m, StorageModels.Enums.ProductCategory.Food, "Подільська смаковинка"),
            new ProductEntity(5, "Desk Table", 10, warehouse1.Guid, 7000m, StorageModels.Enums.ProductCategory.Furniture, "Wooden office desk"),
            new ProductEntity(6, "Wireless Mouse", 25, warehouse1.Guid, 900m, StorageModels.Enums.ProductCategory.Electronics, "Logitech mouse"),
            new ProductEntity(7, "Keyboard Mechanical", 12, warehouse1.Guid, 2500m, StorageModels.Enums.ProductCategory.Electronics, "RGB keyboard"),
            new ProductEntity(8, "Барабанні палички Vater", 6, warehouse1.Guid, 610m, StorageModels.Enums.ProductCategory.Tools, "Мої улюблені"),
            new ProductEntity(9, "Winter Jacket", 20, warehouse1.Guid, 2800m, StorageModels.Enums.ProductCategory.Clothing, "Men jacket"),
            new ProductEntity(10, "Sneakers Nike", 18, warehouse1.Guid, 3200m, StorageModels.Enums.ProductCategory.Clothing, "Sport shoes"),
            new ProductEntity(11, "Круасан", 500, warehouse2.Guid, 45m, StorageModels.Enums.ProductCategory.Food, "Пухкенькі"),
            new ProductEntity(12, "Салат айсберг", 100, warehouse2.Guid, 25.5m, StorageModels.Enums.ProductCategory.Food, "Iceberg")
        ];
       
    }
}
