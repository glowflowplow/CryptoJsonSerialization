using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CryptoJsonSerialization
{
    public static class Utf8JsonReaderExtentions
    {
        public static object GetObject(this Utf8JsonReader reader, Type type = null)
        {
            reader.Read();
            if (reader.TokenType == JsonTokenType.Null)
                return null;

            if (type == null)
                throw new InvalidOperationException("Not allow null type");

            // string is not primitive
            if (type == typeof(string)) return reader.GetString();

            if (type.IsPrimitive)
            {
                if (type == typeof(sbyte)) return reader.GetSByte();
                if (type == typeof(byte)) return reader.GetByte();
                if (type == typeof(short)) return reader.GetInt16();
                if (type == typeof(ushort)) return reader.GetUInt16();
                if (type == typeof(int)) return reader.GetInt32();
                if (type == typeof(uint)) return reader.GetUInt32();
                if (type == typeof(long)) return reader.GetInt64();
                if (type == typeof(ulong)) return reader.GetUInt64();
                if (type == typeof(IntPtr)) throw new NotSupportedException("IntPtr is not supported");
                if (type == typeof(UIntPtr)) throw new NotSupportedException("UIntPtr is not supported");
            }

            if (type.IsArray)
            {
                Type elementType = type.GetElementType();
                IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));
                for (int i = 0; i < list.Count; i++)
                    list.Add(reader.GetObject(type.GetElementType()));
                dynamic array = Array.CreateInstance(elementType, list.Count);
                for (int i = 0; i < list.Count; i++)
                    array[i] = list[i];
                return array;
            }

            Type objectType = null;
            object result = Activator.CreateInstance(objectType);
            if (reader.TokenType == JsonTokenType.StartObject)
            {

            }
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {





                }

            }
            return result;
        }
    }
}
