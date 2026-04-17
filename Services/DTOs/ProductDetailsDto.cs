using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTOs
{
    public class ProductDetailsDto
    {
        // Fields
        private readonly int _id;
        private string _name;
        private string _categoryName;
        private int _quantity;
        private decimal _price;
        private decimal _totalValue;
        private string? _description;
        private string _warehouseName;

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

        public string CategoryName
        {
            get { return _categoryName; }
            set { _categoryName = value; }
        }

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        public decimal Price
        {
            get { return _price; }
            set { _price = value; }
        }

        public decimal TotalValue
        {
            get { return _totalValue; }
            set { _totalValue = value; }
        }

        public string? Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string WarehouseName
        {
            get { return _warehouseName; }
            set { _warehouseName = value; }
        }

        // Constructor
        public ProductDetailsDto(int id, string name, string categoryName, int quantity, decimal price, decimal totalValue, string? description, string warehouseName)
        {
            _id = id;
            _name = name;
            _categoryName = categoryName;
            _quantity = quantity;
            _price = price;
            _totalValue = totalValue;
            _description = description;
            _warehouseName = warehouseName;
        }
    }
}