using System.Web;
using System.Web.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HansKindberg.Web.Simulation.ShimTests
{
    [TestClass]
    public class HttpRuntimeWrapperTest
    {
        #region Methods

        [TestMethod]
        public void ProcessRequest_ShouldCallProcessRequestOfTheWrappedHttpRuntime()
        {
            using(ShimsContext.Create())
            {
                bool processRequestIsCalled = false;
                HttpWorkerRequest httpWorkerRequestValue = null;

                ShimHttpRuntime.ProcessRequestHttpWorkerRequest = delegate(HttpWorkerRequest httpWorkerRequest)
                {
                    httpWorkerRequestValue = httpWorkerRequest;
                    processRequestIsCalled = true;
                };

                HttpWorkerRequest httpWorkerRequestParameter = Mock.Of<HttpWorkerRequest>();

                new HttpRuntimeWrapper().ProcessRequest(httpWorkerRequestParameter);

                Assert.AreEqual(httpWorkerRequestValue, httpWorkerRequestParameter);
                Assert.IsTrue(processRequestIsCalled);
            }
        }

        #endregion
    }
}