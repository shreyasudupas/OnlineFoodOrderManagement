using MenuManagment.Mongo.Domain.Dtos.Inventory;
using System.Collections.Generic;

namespace MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;

public class CategoryDto
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
    public string OpenTime { get; set; }

    public string CloseTime { get; set; }

    public bool Active { get; set; }

    public string ReleaseDateTime { get; set; }
    public List<VendorCategoryMenuDto> MenuItems { get; set; } = new();
}
