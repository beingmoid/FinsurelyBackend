using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace AIB.Common
{
    public class IgnoreCircularReferenceConverter : Newtonsoft.Json.JsonConverter
    {
        private readonly JsonSerializerSettings _settings;

        public IgnoreCircularReferenceConverter(JsonSerializerSettings settings)
        {
            _settings = settings;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(object).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Use the serializer's DefaultValueHandling to ignore circular references
            _settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            _settings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
            _settings.PreserveReferencesHandling = PreserveReferencesHandling.None;

            // Deserialize the JSON with the updated settings
            using (var sr = new StringReader(reader.Value.ToString()))
            using (var jsonReader = new JsonTextReader(sr))
            {
                var newSerializer = JsonSerializer.Create(_settings);
                return newSerializer.Deserialize(jsonReader, objectType);
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Simply write the value to the JSON writer
            serializer.Serialize(writer, value);
        }
    }
}
