using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace CryptoJsonSerialization
{
    public static class Utf8JsonWriterExtentions
    {
        public static void WriteObject(this Utf8JsonWriter writer, object obj)
        {
            if (obj == null)
            {
                writer.WriteNullValue();
                return;
            }

            Type objectType = obj.GetType();
            if (objectType.IsPrimitive || objectType == typeof(string))
            {
                if (Utils.IsNumberType(objectType))
                {
                    writer.WriteNumberValue(decimal.Parse(obj.ToString()));
                }
                else
                {
                    writer.WriteStringValue(obj.ToString());
                }
            }
            else if (objectType.IsArray)
            {
                writer.WriteStartArray();
                foreach (object arrayElement in obj as IEnumerable)
                {
                    writer.WriteObject(arrayElement);
                }
                writer.WriteEndArray();
            }
            else
            {
                writer.WriteStartObject();
                foreach (PropertyInfo propertyInfo in objectType.GetProperties())
                {
                    writer.WritePropertyName(propertyInfo.Name);
                    writer.WriteObject(propertyInfo.GetValue(obj));
                }
                writer.WriteEndObject();
            }
        }

    }
}
