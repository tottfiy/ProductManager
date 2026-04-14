using System;
using System.Collections.Generic;
using ViewModels;

namespace Services
{
    public interface IStorageService
    {
        IEnumerable<WarehouseViewModel> GetWarehouses();

        // Повертає склад по айді
        WarehouseViewModel? GetWarehouseById(int id);

        // Повертає всі продукти з конкретного складу
        IEnumerable<ProductViewModel> GetProductsByWarehouse(Guid warehouseGuid);

        // Повертає конкретний продукт по його айді
        ProductViewModel? GetProductById(int id);
    }
}
