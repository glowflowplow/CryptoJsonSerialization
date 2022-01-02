using CryptoJsonSerialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using ExpectedObjects;

namespace CryptoJsonSerializationTest
{
    [TestClass]
    public class Utf8JsonReaderTest
    {
        [TestMethod]
        public void ReadObject_ReadNullTest()
        {
            string jsonString = @"null";
            var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonString));
            object obj = reader.GetObject(typeof(string));
            Assert.IsNull(obj);
        }

        [TestMethod]
        public void ReadObject_ReadNumberTest()
        {
            string jsonString = @"1";
            var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonString));
            object obj = reader.GetObject(typeof(int));
            Assert.AreEqual(1, obj);
        }

        [TestMethod]
        public void ReadObject_ReadStringTest()
        {
            string jsonString = @"""test message""";
            var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonString));
            object obj = reader.GetObject(typeof(string));
            Assert.AreEqual(@"test message", obj);
        }

        [TestMethod]
        public void ReadObject_ReadArrayTest()
        {
            var ext = new[] { 1, 2, 3 };
            string jsonString = @"[1,2,3]";
            var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonString));
            object act = reader.GetObject(typeof(int[]));
            ext.ToExpectedObject().ShouldEqual(act);
        }

        [TestMethod]
        public void ReadObject_ReadObjectTest()
        {
            string jsonString = @"{""PropetyA"":""test"",""PropertyB"":""message""}";
            var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonString));
            object obj = reader.GetObject(typeof(object));
            Assert.AreEqual(new { PropertyA = "test", PropertyB = "message" }, obj);
        }
    }
}
