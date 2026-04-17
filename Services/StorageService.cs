using Repositories;
using Services.DTOs;
using StorageModels.Entities;
using System.Collections.Generic;

namespace Services
{
    public class StorageService : IStorageService
    {
        private readonly IStorageRepository _storageRepository;

        public StorageService(IStorageRepository storageRepository)
        {
            _storageRepository = storageRepository;
        }

        public IReadOnlyCollection<WarehouseListDto> GetWarehouseList()
        {
            var warehouses = _storageRepository.GetWarehouses();
            var products = _storageRepository.GetProducts();

            List<WarehouseListDto> warehouseDtos = new List<WarehouseListDto>();

            foreach (var warehouse in warehouses)
            {
                int productCount = 0;
                decimal totalValue = 0;

                foreach (var product in products)
                {
                    if (product.StorageGuid == warehouse.Guid)
                    {
                        productCount++;
                        totalValue += product.Price * product.Quantity;
                    }
                }

                WarehouseListDto warehouseDto = new WarehouseListDto(
                    warehouse.Id,
                    warehouse.Name,
                    warehouse.Location.ToString(),
                    productCount,
                    totalValue
                );

                warehouseDtos.Add(warehouseDto);
            }

            return warehouseDtos;
        }

        public WarehouseDetailsDto? GetWarehouseDetails(int warehouseId)
        {
            WarehouseEntity? warehouse = _storageRepository.GetWarehouseById(warehouseId);

            if (warehouse == null)
            {
                return null;
            }

            List<ProductListDto> productDtos = new List<ProductListDto>();
            decimal totalValue = 0;

            foreach (var product in _storageRepository.GetProductsByWarehouse(warehouse.Guid))
            {
                decimal productTotalValue = product.Price * product.Quantity;

                ProductListDto productDto = new ProductListDto(
                    product.Id,
                    product.Name,
                    product.ProductCategory.ToString(),
                    product.Quantity,
                    product.Price,
                    productTotalValue
                );

                productDtos.Add(productDto);
                totalValue += productTotalValue;
            }

            WarehouseDetailsDto warehouseDetailsDto = new WarehouseDetailsDto(
                warehouse.Id,
                warehouse.Name,
                warehouse.Location.ToString(),
                totalValue,
                productDtos
            );

            return warehouseDetailsDto;
        }

        public ProductDetailsDto? GetProductDetails(int productId)
        {
            ProductEntity? product = _storageRepository.GetProductById(productId);

            if (product == null)
            {
                return null;
            }

            string warehouseName = "Невідомий склад";

            foreach (var warehouse in _storageRepository.GetWarehouses())
            {
                if (warehouse.Guid == product.StorageGuid)
                {
                    warehouseName = warehouse.Name;
                    break;
                }
            }

            string? description = product.Description;

            if (string.IsNullOrWhiteSpace(description))
            {
                description = "—";
            }

            ProductDetailsDto productDetailsDto = new ProductDetailsDto(
                product.Id,
                product.Name,
                product.ProductCategory.ToString(),
                product.Quantity,
                product.Price,
                product.Price * product.Quantity,
                description,
                warehouseName
            );

            return productDetailsDto;
        }
    }
}