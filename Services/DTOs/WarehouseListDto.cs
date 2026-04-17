using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTOs
{
    public class WarehouseListDto
    {
        // Fields
        private readonly int _id;
        private string _name;
        private string _locationName;
        private int _productCount;
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

        public string LocationName
        {
            get { return _locationName; }
            set { _locationName = value; }
        }

        public int ProductCount
        {
            get { return _productCount; }
            set { _productCount = value; }
        }

        public decimal TotalValue
        {
            get { return _totalValue; }
            set { _totalValue = value; }
        }

        // Constructor
        public WarehouseListDto(int id, string name, string locationName, int productCount, decimal totalValue)
        {
            _id = id;
            _name = name;
            _locationName = locationName;
            _productCount = productCount;
            _totalValue = totalValue;
        }
    }
}