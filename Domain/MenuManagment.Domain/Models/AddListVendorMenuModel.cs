using Microsoft.AspNetCore.Http;

namespace MenuManagment.Mongo.Domain.Models
{
    public class AddListVendorMenuModel
    {
        public IFormFile UploadFile { get; set; }

        public string VendorId { get; set; }
    }
}
