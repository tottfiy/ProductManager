using StorageModels.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorageModels.Entities
{
    public class Product
    {
        // Fields
        private readonly Guid _guid;
        public int _id;
        public Guid _storageGuid;
        private string _name;
        private int _quantity;
        private double _price;
        private ProductCategory _productCategory;
        private string? _description;

        // Properties
        public Guid Guid
        {
            get { return _guid; }
        }
        public Guid StorageGuid
        {
            get { return _storageGuid; }
            private set { _storageGuid = value; }
        }
        public int Id
        {
            get { return _id; }
            private set { _id = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }
        public ProductCategory ProductCategory
        {
            get { return _productCategory; }
            set { _productCategory = value; }
        }
        public string? Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public double TotalValue
        {
            get { return Price * Quantity; }
        }

        // Constructor
        public Product(int id, string name, int quantity, Guid storageGuid, double price, ProductCategory productCategory, string? description = null)
        {
            _guid = Guid.NewGuid();
            _id = id;
            _storageGuid = storageGuid;
            _name = name;
            _quantity = quantity;
            _price = price;
            _productCategory = productCategory;
            _description = description;
        }
    }
}
