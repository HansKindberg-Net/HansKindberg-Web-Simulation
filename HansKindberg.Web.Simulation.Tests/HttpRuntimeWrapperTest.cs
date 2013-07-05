using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HansKindberg.Web.Simulation.Tests
{
    [TestClass]
    public class HttpRuntimeWrapperTest
    {
        #region Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ProcessRequest_IfTheHttpWorkerRequestParameterValueIsNull_ShouldThrowAnArgumentNullException()
        {
            new HttpRuntimeWrapper().ProcessRequest(null);
        }

        #endregion
    }
}