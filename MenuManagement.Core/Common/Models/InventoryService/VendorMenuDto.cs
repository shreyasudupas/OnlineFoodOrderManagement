﻿using System.Collections.Generic;

namespace MenuManagement.Core.Common.Models.InventoryService
{
    public class VendorMenuDto
    {
        public string Id { get; set; }

        public string VendorId { get; set; }

        public string ItemName { get; set; }

        public string ImageLocation { get; set; }

        public string FoodType { get; set; }

        public string Category { get; set; }

        public double Price { get; set; }

        public int Discount { get; set; }

        public int Rating { get; set; }

        public bool Active { get; set; }
    }
}