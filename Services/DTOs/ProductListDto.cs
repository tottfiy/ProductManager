using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTOs
{
    public class ProductListDto
    {
        // Fields
        private readonly int _id;
        private string _name;
        private string _categoryName;
        private int _quantity;
        private decimal _price;
        private decimal _totalValue;

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

        // Constructor
        public ProductListDto(int id, string name, string categoryName, int quantity, decimal price, decimal totalValue)
        {
            _id = id;
            _name = name;
            _categoryName = categoryName;
            _quantity = quantity;
            _price = price;
            _totalValue = totalValue;
        }
    }
}