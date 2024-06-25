using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System.IO;

namespace Inventory.Microservice.Core.Common.SchemaGenerator
{
    public static class JsonSchmeaGeneratorFunction
    {
        public static T CheckIfFileContentMatchesSchema<T>(string fileContent) where T : class
        {
            JsonTextReader jsonReader = new JsonTextReader(new StringReader(fileContent));

            JSchemaValidatingReader validatingReader = new JSchemaValidatingReader(jsonReader);
            validatingReader.Schema = GetSchema<T>();

            JsonSerializer serializer = new JsonSerializer();
            var vendorMenus = serializer.Deserialize<T>(validatingReader);

            return vendorMenus;
        }

        public static JSchema GetSchema<T>() where T : class
        {
            JSchemaGenerator generator = new JSchemaGenerator()
            {
                DefaultRequired = Required.DisallowNull
            };
            JSchema schema = generator.Generate(typeof(T));
            return schema;
        }
    }
}
