using StorageModels.Entities;
using StorageModels.Enums;

namespace ViewModels
{
    /// View model for working with Product entity (display, edit, calculated fields)
    public class ProductViewModel
    {
        private readonly ProductEntity _product;

        public ProductViewModel(ProductEntity product)
        {
            _product = product;
        }

        public Guid Guid => _product.Guid;
        public int Id => _product.Id;
        public Guid StorageGuid => _product.StorageGuid;

        public string Name
        {
            get => _product.Name;
            set => _product.Name = value;
        }

        public int Quantity
        {
            get => _product.Quantity;
            set => _product.Quantity = value;
        }

        public decimal Price
        {
            get => _product.Price;
            set => _product.Price = value;
        }

        public ProductCategory ProductCategory
        {
            get => _product.ProductCategory;
            set => _product.ProductCategory = value;
        }

        public string? Description
        {
            get => _product.Description;
            set => _product.Description = value;
        }

        public decimal TotalValue => Quantity * Price;

        public void DisplayShort()
        {
            Console.WriteLine($"{Id}. {Name} | Qty: {Quantity} | Total: {TotalValue}");
        }

        public void DisplayFull()
        {
            Console.WriteLine($"Id: {Id}");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Category: {ProductCategory}");
            Console.WriteLine($"Quantity: {Quantity}");
            Console.WriteLine($"Price: {Price}");
            Console.WriteLine($"Total Value: {TotalValue}");
            Console.WriteLine($"Description: {Description}");
        }
    }
}