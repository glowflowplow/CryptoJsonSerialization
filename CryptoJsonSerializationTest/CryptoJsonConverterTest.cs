using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CryptoJsonSerialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using ExpectedObjects;

namespace CryptoJsonSerializationTest
{
    [TestClass]
    public class CryptoJsonConverterTest
    {
        [TestMethod]
        public void SerializeAndDeserializeStringTest()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new CryptoJsonConverter());

            var aes = System.Security.Cryptography.Aes.Create();
            aes.GenerateKey();
            CryptoJsonConverter.AesKey = aes.Key;

            string ext = "test message";
            string serialized = JsonSerializer.Serialize(ext, options);
            string act = (string)JsonSerializer.Deserialize(serialized, typeof(string), options);
            ext.ToExpectedObject().ShouldEqual(act);
        }

        [TestMethod]
        public void SerializeAndDeserializeObjectTest()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new CryptoJsonConverter());

            var aes = System.Security.Cryptography.Aes.Create();
            aes.GenerateKey();
            CryptoJsonConverter.AesKey = aes.Key;

            var ext = new TestClass2("test messsage");

            string serialized = JsonSerializer.Serialize(ext, options);
            TestClass2 act = (TestClass2)JsonSerializer.Deserialize(serialized, typeof(TestClass2), options);
            ext.ToExpectedObject().ShouldEqual(act);
        }

        [TestMethod]
        public void SerializeByAttribute()
        {
            var ext = new TestClass1("CryptoProperty", "NormalProperty");
            string serialized = JsonSerializer.Serialize(ext);
            TestClass1 act = (TestClass1)JsonSerializer.Deserialize(serialized, typeof(TestClass1));
            ext.ToExpectedObject().ShouldEqual(act);
        }

        [TestMethod]
        public void SerializeByAttribute2()
        {
            var ext = new TestClass3("CryptoProperty");
            string serialized = JsonSerializer.Serialize(ext);
            object act = JsonSerializer.Deserialize(serialized, typeof(TestClass3));
            ext.ToExpectedObject().ShouldEqual(act);
        }

        [TestMethod]
        public void NoConverter()
        {
            TestClass4 ext = new TestClass4("CryptoProperty");
            string serialized = JsonSerializer.Serialize(ext);
            TestClass4 act = (TestClass4)JsonSerializer.Deserialize(serialized, typeof(TestClass4));
            ext.ToExpectedObject().ShouldEqual(act);
        }

        [TestMethod]
        public void EqualObject()
        {
            var obj1 = new { member = "fuck" };
            var obj2 = new { member = "fuck" };
            Assert.AreEqual(obj1, obj2);
        }

        private class TestClass1
        {
            [JsonConverter(typeof(CryptoJsonConverter))]
            public string CryptoProperty { get; set; }
            public string NormalProperty { get; set; }

            public TestClass1() { }

            public TestClass1(string cryptoProperty, string normalProperty)
            {
                CryptoProperty = cryptoProperty;
                NormalProperty = normalProperty;
            }
        }
        private class TestClass2
        {
            public string NormalProperty { get; set; }
            public TestClass2() { }
            public TestClass2(string normalProperty)
            {
                NormalProperty = normalProperty;
            }
        }

        private class TestClass3
        {
            public TestClass3() { }
            public TestClass3(string cryptoProperty)
            {
                CryptoProperty = cryptoProperty;
            }

            [JsonConverter(typeof(CryptoJsonConverter.CryptoPrimitiveConverter<string>))]
            public string CryptoProperty { get; set; }

        }

        public class TestClass4
        {
            public TestClass4() { }
            public TestClass4(string cryptoProperty)
            {
                PassedProperty = cryptoProperty;
            }

            //[JsonConverter(typeof(PassJsonConverter))]
            public string PassedProperty { get; set; }

        }
    }
}
