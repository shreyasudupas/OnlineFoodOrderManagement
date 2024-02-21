namespace MenuMangement.HttpClient.Domain.Exceptions
{
    public class GraphQLException : Exception
    {
        public GraphQLException(string message,
            Exception innerException
            ) : base(message, innerException) 
        {
        }
    }
}
