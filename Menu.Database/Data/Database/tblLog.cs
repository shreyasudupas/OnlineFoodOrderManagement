namespace MenuDatabase.Data.Database
{
    public class tblLog
    {
        public long LogId { get; set; }
        public string ControllerName { get; set; }
        public string ActionMethod { get; set; }
        public string ErrorMessage { get; set; }
    }
}