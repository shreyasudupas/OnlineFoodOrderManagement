namespace MenuOrder.Shared.Models
{
    public class WelcomeVendorModel : MailBody
    {
        public string VendorName { get; set; }
        public string VendorEmail { get; set; }
        public string Username { get; set; }
    }
}
