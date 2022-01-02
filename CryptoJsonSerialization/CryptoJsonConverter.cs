using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CryptoJsonSerialization
{
    public class CryptoJsonConverter : JsonConverterFactory
    {
        private readonly static Aes Aes = Aes.Create();
        public static byte[] AesKey
        {
            get { return Aes.Key; }
            set { Aes.Key = value; }
        }
        public static string AesKeyAsString
        {
            get { return ByteArrayConverter.ToString(AesKey); }
            set { AesKey = ByteArrayConverter.ToByteArray(value); }
        }

        public override bool CanConvert(Type typeToConvert)
        {
            if (typeToConvert == typeof(string)) return true;
            if (!typeToConvert.IsPrimitive) return false;
            if (typeToConvert == typeof(IntPtr)) return false;
            if (typeToConvert == typeof(UIntPtr)) return false;
            return true;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            JsonConverter converter = (JsonConverter)Activator.CreateInstance(typeof(CryptoPrimitiveConverter<>).MakeGenericType(typeToConvert));
            return converter;
        }

        public class CryptoPrimitiveConverter<T> : JsonConverter<T>
        {
            public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType != JsonTokenType.StartObject)
                    throw new JsonException("Invalid TokenType");

                var element = readCryptoElement(ref reader);

                if (typeToConvert.ToString() != element.Type) throw new JsonException();

                Aes.IV = ByteArrayConverter.ToByteArray(element.Iv);
                var decryptor = Aes.CreateDecryptor();
                byte[] encryptedByteArrayValue = ByteArrayConverter.ToByteArray(element.Value);
                string strValue = Encoding.UTF8.GetString(decryptor.TransformFinalBlock(encryptedByteArrayValue, 0, encryptedByteArrayValue.Length));
                if (IsNumberType(typeToConvert))
                    return (T)(object)decimal.Parse(strValue);
                else if (typeToConvert == typeof(string))
                    return (T)(object)(strValue);
                else
                    return (T)(object)strValue;
            }

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            {
                CryptoElement element = new CryptoElement();
                element.Type = typeof(T).ToString();
                Aes.GenerateIV();
                var encryptor = Aes.CreateEncryptor();
                var byteValue = Encoding.UTF8.GetBytes(value.ToString());
                element.Value = ByteArrayConverter.ToString(encryptor.TransformFinalBlock(byteValue, 0, byteValue.Length));
                element.Iv = ByteArrayConverter.ToString(Aes.IV);

                writeCryptoElement(writer, element);
            }

            private class CryptoElement
            {
                public string Value { get; set; }
                public string Type { get; set; }
                public string Iv { get; set; }
            }

            private void writeCryptoElement(Utf8JsonWriter writer, CryptoElement element)
            {
                writer.WriteStartObject();
                writer.WriteString("Value", element.Value);
                writer.WriteString("Type", element.Type);
                writer.WriteString("Iv", element.Iv);
                writer.WriteEndObject();
            }

            private CryptoElement readCryptoElement(ref Utf8JsonReader reader)
            {
                var element = new CryptoElement();
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject) break;
                    if (reader.TokenType != JsonTokenType.PropertyName) continue;

                    string propName = reader.GetString();

                    if (!reader.Read()) throw new JsonException();
                    switch (propName)
                    {
                        case "Value":
                            element.Value = reader.GetString();
                            break;
                        case "Type":
                            element.Type = reader.GetString();
                            break;
                        case "Iv":
                            element.Iv = reader.GetString();
                            break;
                    }
                }
                return element;
            }
        }

        public static bool IsNumberType(Type type)
        {
            var numTypes = new Type[] {
                        typeof(byte),
                        typeof(sbyte),
                        typeof(short),
                        typeof(ushort),
                        typeof(int),
                        typeof(uint),
                        typeof(long),
                        typeof(ulong),
                        typeof(float),
                        typeof(double),
                        typeof(decimal)
                    };
            return numTypes.Contains(type);
        }
    }
}
