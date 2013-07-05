using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HansKindberg.Web.Simulation.IntegrationTests
{
    [TestClass]
    public class HttpApplicationManagerTest
    {
        #region Methods

        [TestMethod]
        public void ShouldBeSerializable()
        {
            HttpApplicationManager httpApplicationManager = new HttpApplicationManager();
            HttpApplicationManager deserializedHttpApplicationManager;

            BinaryFormatter formatter = new BinaryFormatter();

            using(MemoryStream memoryStream = new MemoryStream())
            {
                formatter.Serialize(memoryStream, httpApplicationManager);

                memoryStream.Position = 0;

                deserializedHttpApplicationManager = (HttpApplicationManager) formatter.Deserialize(memoryStream);
            }

            Assert.IsNotNull(deserializedHttpApplicationManager);
            Assert.AreNotEqual(httpApplicationManager, deserializedHttpApplicationManager);
        }

        #endregion
    }
}