using StorageModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using ViewModels;

namespace Services
{
    public class StorageService
    {
        public IEnumerable<WarehouseViewModel> GetWarehouses()
        {
            return FakeStorage.Warehouses.Select(
                warehouse => new WarehouseViewModel(warehouse)).ToList();
        }

        public WarehouseViewModel? GetWarehouseById(int id)
        {
            var warehouse = FakeStorage.Warehouses.FirstOrDefault(w => w.Id == id);

            if (warehouse == null)
                return null;

            var vm = new WarehouseViewModel(warehouse);
            vm.LoadProducts(GetProductsByWarehouse(vm.Guid)); 
            return vm;
        }

        public IEnumerable<ProductViewModel> GetProductsByWarehouse(Guid warehouseGuid)
        {
            return FakeStorage.Products
                .Where(p => p.StorageGuid == warehouseGuid)
                .Select(product => new ProductViewModel(product))
                .ToList();
        }
        public ProductViewModel? GetProductById(int id)
        {
            var product = FakeStorage.Products.FirstOrDefault(p => p.Id == id);
            return product != null ? new ProductViewModel(product) : null;
        }

    }
}