using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTOs
{
    public class WarehouseDetailsDto
    {
        // Fields
        private readonly int _id;
        private string _name;
        private string _locationName;
        private decimal _totalValue;
        private List<ProductListDto> _products;

        // Properties
        public int Id
        {
            get { return _id; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string LocationName
        {
            get { return _locationName; }
            set { _locationName = value; }
        }

        public decimal TotalValue
        {
            get { return _totalValue; }
            set { _totalValue = value; }
        }

        public IReadOnlyCollection<ProductListDto> Products
        {
            get { return _products.AsReadOnly(); }
        }

        // Constructor
        public WarehouseDetailsDto(int id, string name, string locationName, decimal totalValue, List<ProductListDto> products)
        {
            _id = id;
            _name = name;
            _locationName = locationName;
            _totalValue = totalValue;
            _products = products ?? new List<ProductListDto>();
        }
    }
}