using System.IO;
using System.Web;
using HansKindberg.Web.Simulation.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HansKindberg.Web.Simulation.IntegrationTests.Serialization
{
    [TestClass]
    public class DefaultSerializableObjectFactoryTest
    {
        #region Methods

        [TestMethod]
        public void CreateInstance_IfTheTypeParameterIsOfTypeHttpContext_ShouldReturnAnInstanceOfTypeHttpContext()
        {
            Assert.IsNotNull(new DefaultSerializableObjectFactory().CreateInstance<HttpContext>());
        }

        [TestMethod]
        public void CreateInstance_IfTheTypeParameterIsOfTypeHttpRequest_ShouldReturnAnInstanceOfTypeHttpRequest()
        {
            Assert.IsNotNull(new DefaultSerializableObjectFactory().CreateInstance<HttpRequest>());
        }

        [TestMethod]
        public void CreateInstance_IfTheTypeParameterIsOfTypeHttpResponse_ShouldReturnAnInstanceOfTypeHttpResponse()
        {
            Assert.IsNotNull(new DefaultSerializableObjectFactory().CreateInstance<HttpResponse>());
        }

        [TestMethod]
        public void CreateInstance_IfTheTypeParameterIsOfTypeTextWriter_ShouldReturnAnInstanceOfTypeTextWriter()
        {
            Assert.IsNotNull(new DefaultSerializableObjectFactory().CreateInstance<TextWriter>());
        }

        #endregion
    }
}