using StorageModels.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorageModels.Entities
{
    public class WarehouseEntity
    {
        // Fields
        private readonly Guid _guid;
        private readonly int _id;
        private string _name;
        private Location _location;

        // Properties
        public Guid Guid
        {
            get { return _guid; }
        }

        public int Id
        {
            get { return _id; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Location Location
        {
            get { return _location; }
            set { _location = value; }
        }
        
        // Constructor
        public WarehouseEntity(int id, string name, Location location)
        {
            _guid = Guid.NewGuid();
            _id = id;
            _name = name;
            _location = location;
        }
    }
}
