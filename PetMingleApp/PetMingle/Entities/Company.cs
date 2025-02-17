﻿namespace PetMingle.Entities
{
    public class Company
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int StockQuantity { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
