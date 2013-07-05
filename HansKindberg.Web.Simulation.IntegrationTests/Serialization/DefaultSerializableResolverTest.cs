using System.Linq;
using System.Web;
using HansKindberg.Web.Simulation.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HansKindberg.Web.Simulation.IntegrationTests.Serialization
{
    [TestClass]
    public class DefaultSerializableResolverTest
    {
        #region Methods

        [TestMethod]
        public void GetFields_IfTheTypeParameterValueIsOfTypeHttpContext_ShouldReturnSixtyEightItems()
        {
            Assert.AreEqual(68, new DefaultSerializableResolver().GetFields(typeof(HttpContext)).Count());
        }

        [TestMethod]
        public void GetFields_IfTheTypeParameterValueIsOfTypeHttpRequest_ShouldReturnFiftyOneItems()
        {
            Assert.AreEqual(51, new DefaultSerializableResolver().GetFields(typeof(HttpRequest)).Count());
        }

        #endregion
    }
}