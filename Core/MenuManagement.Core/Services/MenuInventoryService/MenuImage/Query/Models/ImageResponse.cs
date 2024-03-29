﻿using System.Collections.Generic;

namespace Inventory.Microservice.Core.Services.MenuInventoryService.MenuImage.Query.Models
{
    public class ImageResponse
    {
        public ImageResponse()
        {
            ImageData = new List<ImageDataModel>();
        }
        public int TotalRecord { get; set; }
        public List<ImageDataModel> ImageData { get; set; }
    }
}
