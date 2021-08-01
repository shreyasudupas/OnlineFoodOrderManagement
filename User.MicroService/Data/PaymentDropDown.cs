namespace Identity.MicroService.Data
{
    public class PaymentDropDown : IEntityBase
    {
        public long Id { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }
        public string Code { get; set; }
    }
}
