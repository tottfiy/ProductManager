using StorageModels.Entities;
using StorageModels.Enums;

namespace ViewModels
{
    /// View model for working with Warehouse entity
    public class WarehouseViewModel
    {
        private readonly WarehouseEntity _warehouse;
        private List<ProductViewModel> _products;

        public WarehouseViewModel(WarehouseEntity warehouse)
        {
            _warehouse = warehouse;
            _products = new List<ProductViewModel>();
        }

        public Guid Guid => _warehouse.Guid;
        public int Id => _warehouse.Id;

        public string Name
        {
            get => _warehouse.Name;
            set => _warehouse.Name = value;
        }

        public Location Location
        {
            get => _warehouse.Location;
            set => _warehouse.Location = value;
        }

        public IReadOnlyCollection<ProductViewModel> Products
            => _products.AsReadOnly();

        public decimal TotalValue
        {
            get
            {
                if (_products == null || _products.Count == 0)
                    return 0m;

                decimal sum = 0m;
                foreach (var product in _products)
                    sum += product.TotalValue;

                return sum;
            }
        }


        public void LoadProducts(IEnumerable<ProductViewModel> products)
        {
            _products.Clear();
            _products.AddRange(products);
        }

        public void Display()
        {
            Console.WriteLine($"[{Id}]: {Name}");
        }

        public void DisplayDetailed()
        {
            Console.WriteLine($"[{Id}]: {Name}");
            Console.WriteLine($"Location: {Location}");
            Console.WriteLine($"Total value: {TotalValue}");
        }
        
        public void DisplayWithProducts()
        {
            Display();
            Console.WriteLine("Products:");

            if (!_products.Any())
            {
                Console.WriteLine("No products available.");
                return;
            }

            foreach (var product in _products)
            {
                product.DisplayShort();
            }
        }
    }
}