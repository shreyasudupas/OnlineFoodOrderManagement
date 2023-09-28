namespace MenuManagment.Mongo.Domain.Enum
{
    public enum OrderStatusEnum
    {
        WaitingOnVendorAccept,
        AcceptedByVendor,
        Processing,
        Ready,
        Done
    }
}
