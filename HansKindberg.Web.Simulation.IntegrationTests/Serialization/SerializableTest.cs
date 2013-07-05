using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using HansKindberg.Web.Simulation.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HansKindberg.Web.Simulation.IntegrationTests.Serialization
{
    [TestClass]
    public class SerializableTest
    {
        //[TestMethod]
        //public void SerializableHttpContext_IfTheSerializableObjectFactoryInstanceCanCreateAnObjectOfTypeHttpContext_ShouldBeSerializable()
        //{
        //    const string page = "Default.html";
        //    const string rawUrl = "/" + page;
        //    using (StringWriter output = new StringWriter(CultureInfo.InvariantCulture))
        //    {
        //        HttpContext httpContext = new HttpContext(new SimpleWorkerRequest("/", AppDomain.CurrentDomain.BaseDirectory, page, string.Empty, output));
        //        Assert.AreEqual(rawUrl, httpContext.Request.RawUrl);
        //        Serializable<HttpContext> serializableHttpContext = new Serializable<HttpContext>(httpContext);
        //        BinaryFormatter formatter = new BinaryFormatter();
        //        using (MemoryStream memoryStream = new MemoryStream())
        //        {
        //            formatter.Serialize(memoryStream, serializableHttpContext);
        //            memoryStream.Position = 0;
        //            Serializable<HttpContext> deserializedSerializableHttpContext = (Serializable<HttpContext>)formatter.Deserialize(memoryStream);
        //            HttpContext deserializedHttpContext = deserializedSerializableHttpContext.Instance;
        //            Assert.AreEqual(rawUrl, deserializedHttpContext.Request.RawUrl);
        //        }
        //        //using(FileStream fileStream = new FileStream(Path.GetTempFileName(), FileMode.OpenOrCreate))
        //        //{
        //        //    formatter.Serialize(fileStream, serializableHttpContext);
        //        //    fileStream.Position = 0;
        //        //    Serializable<HttpContext> deserializedSerializableHttpContext = (Serializable<HttpContext>) formatter.Deserialize(fileStream);
        //        //    HttpContext deserializedHttpContext = deserializedSerializableHttpContext.Instance;
        //        //    Assert.AreEqual(rawUrl, deserializedHttpContext.Request.RawUrl);
        //        //}
        //    }
        //}

        #region Methods

        [TestMethod]
        public void SerializableHttpRequest_IfTheSerializableObjectFactoryInstanceCanCreateAnObjectOfTypeHttpRequest_ShouldBeSerializable()
        {
            const string filename = "Default.html";
            const string relativePath = "/" + filename;

            HttpRequest httpRequest = new HttpRequest(filename, "http://localhost" + relativePath, string.Empty);
            Assert.AreEqual(relativePath, httpRequest.FilePath);
            Assert.AreEqual(relativePath, httpRequest.RawUrl);
            Assert.AreEqual("http://localhost" + relativePath, httpRequest.Url.ToString());
            Serializable<HttpRequest> serializableHttpRequest = new Serializable<HttpRequest>(httpRequest);
            BinaryFormatter formatter = new BinaryFormatter();
            using(MemoryStream memoryStream = new MemoryStream())
            {
                formatter.Serialize(memoryStream, serializableHttpRequest);
                memoryStream.Position = 0;
                Serializable<HttpRequest> deserializedSerializableHttpRequest = (Serializable<HttpRequest>) formatter.Deserialize(memoryStream);
                HttpRequest deserializedHttpRequest = deserializedSerializableHttpRequest.Instance;
                Assert.AreEqual(httpRequest.FilePath, deserializedHttpRequest.FilePath);
                Assert.AreEqual(httpRequest.RawUrl, deserializedHttpRequest.RawUrl);
                Assert.AreEqual(httpRequest.Url.ToString(), deserializedHttpRequest.Url.ToString());
            }
        }

        #endregion
    }
}