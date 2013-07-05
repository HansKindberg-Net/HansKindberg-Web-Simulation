using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HansKindberg.Web.Simulation.IntegrationTests
{
    [TestClass]
    public class HttpRuntimeWrapperTest
    {
        #region Methods

        [TestMethod]
        public void ShouldBeSerializable()
        {
            HttpRuntimeWrapper httpRuntimeWrapper = new HttpRuntimeWrapper();
            HttpRuntimeWrapper deserializedHttpRuntimeWrapper;

            BinaryFormatter formatter = new BinaryFormatter();

            using(MemoryStream memoryStream = new MemoryStream())
            {
                formatter.Serialize(memoryStream, httpRuntimeWrapper);

                memoryStream.Position = 0;

                deserializedHttpRuntimeWrapper = (HttpRuntimeWrapper) formatter.Deserialize(memoryStream);
            }

            Assert.IsNotNull(deserializedHttpRuntimeWrapper);
            Assert.AreNotEqual(httpRuntimeWrapper, deserializedHttpRuntimeWrapper);
        }

        #endregion
    }
}