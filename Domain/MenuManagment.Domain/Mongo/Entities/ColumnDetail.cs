namespace MenuManagment.Domain.Mongo.Entities
{
    public class ColumnDetail : CommonDetails
    {
        public string ColumnName { get; set; }


        public string ColumnDescription { get; set; }

        public string DisplayName { get; set; }

        public string Display { get; set; }
    }

    public class CommonDetails
    {
        public string PropertyType { get; set; }
    }
}
