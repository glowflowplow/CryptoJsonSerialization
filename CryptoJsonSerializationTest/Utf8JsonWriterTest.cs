using CryptoJsonSerialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace CryptoJsonSerializationTest
{
    [TestClass]
    public class Utf8JsonWriterTest
    {
        //[TestMethod]
        //public void SampleTest()
        //{
        //    using (var stream = new FileStream(@"C:\Users\plow\source\repos\CryptoJsonSerialization\CryptoJsonSerializationTest\sampleout.txt", FileMode.CreateNew))
        //    using (var writer = new Utf8JsonWriter(stream))
        //    {
        //        writer.WriteNullValue();
        //    }
        //}

        [TestMethod]
        public void WriteObject_WriteNullTest()
        {
            var stream = new MemoryStream();
            using (var writer = new Utf8JsonWriter(stream))
            {
                writer.WriteObject(null);
            }
            stream.Close();
            string actual = Encoding.UTF8.GetString(stream.ToArray());
            Assert.AreEqual(@"null", actual);
        }

        [TestMethod]
        public void WriteObject_WriteNumberTest()
        {
            var stream = new MemoryStream();
            using (var writer = new Utf8JsonWriter(stream))
            {
                writer.WriteObject(1);
            }
            stream.Close();
            string actual = Encoding.UTF8.GetString(stream.ToArray());
            Assert.AreEqual(@"1", actual);
        }

        [TestMethod]
        public void WriteObject_WriteStringTest()
        {
            var stream = new MemoryStream();
            using (var writer = new Utf8JsonWriter(stream))
            {
                writer.WriteObject("this is test message");
            }
            stream.Close();
            string actual = Encoding.UTF8.GetString(stream.ToArray());
            Assert.AreEqual(@"""this is test message""", actual);
        }

        [TestMethod]
        public void WriteObject_WriteArrayTest()
        {
            var stream = new MemoryStream();
            using (var writer = new Utf8JsonWriter(stream))
            {
                writer.WriteObject(new[] { 1, 2, 3 });
            }
            stream.Close();
            string actual = Encoding.UTF8.GetString(stream.ToArray());
            Assert.AreEqual(@"[1,2,3]", actual);
        }

        [TestMethod]
        public void WriteObject_WriteObjectTest()
        {
            var stream = new MemoryStream();
            using (var writer = new Utf8JsonWriter(stream))
            {
                writer.WriteObject(new { Name = "John", Age = 30 });
            }
            stream.Close();
            string actual = Encoding.UTF8.GetString(stream.ToArray());
            Assert.AreEqual(actual, @"{""Name"":""John"",""Age"":30}");
        }

        [TestMethod]
        public void WriteObject_WriteObject2Test()
        {
            var stream = new MemoryStream();
            using (var writer = new Utf8JsonWriter(stream))
            {
                writer.WriteObject(new { Name = "John", Age = 30, Father = new { Name = "Ben" } });
            }
            stream.Close();
            string actual = Encoding.UTF8.GetString(stream.ToArray());
            Assert.AreEqual(actual, @"{""Name"":""John"",""Age"":30,""Father"":{""Name"":""Ben""}}");
        }
    }
}
