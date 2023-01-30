using MongoDB.Driver;

namespace MongoDb.Shared.Persistance.Extensions
{
    public static class MongoExtension
    {
        public static UpdateDefinition<T> ApplyMultiFields<T>(this UpdateDefinitionBuilder<T> builder, T obj)
        {
            var properties = obj.GetType().GetProperties();
            UpdateDefinition<T> definition = null;

            foreach (var property in properties)
            {
                if (definition == null)
                {
                    definition = builder.Set(property.Name, property.GetValue(obj));
                }
                else
                {
                    definition = definition.Set(property.Name, property.GetValue(obj));
                }
            }

            return definition;
        }
    }
}
