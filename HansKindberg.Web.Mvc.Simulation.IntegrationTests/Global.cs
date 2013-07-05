using System.Diagnostics.CodeAnalysis;
using System.IO;
using HansKindberg.Web.Mvc.Simulation.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HansKindberg.Web.Mvc.Simulation.IntegrationTests
{
    [TestClass]
    [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Global")]
    public static class Global
    {
        #region Fields

        private static readonly MvcApplicationHostProxy _mvcApplicationHostProxy;
        private static readonly IMvcApplicationHostProxyFactory _mvcApplicationHostProxyFactory;
        private static readonly string _testApplicationProjectPath;
        private static readonly string _visualStudioSolutionPath;

        #endregion

        #region Constructors

        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static Global()
        {
            _visualStudioSolutionPath = Path.GetFullPath("..\\..\\..");
            _testApplicationProjectPath = Path.Combine(_visualStudioSolutionPath, "HansKindberg.Web.Mvc.Simulation.Application");
            _mvcApplicationHostProxyFactory = new MvcApplicationHostProxyFactory();
            _mvcApplicationHostProxy = _mvcApplicationHostProxyFactory.Create(_testApplicationProjectPath);
        }

        #endregion

        #region Properties

        public static MvcApplicationHostProxy MvcApplicationHostProxy
        {
            get { return _mvcApplicationHostProxy; }
        }

        public static IMvcApplicationHostProxyFactory MvcApplicationHostProxyFactory
        {
            get { return _mvcApplicationHostProxyFactory; }
        }

        public static string TestApplicationProjectPath
        {
            get { return _testApplicationProjectPath; }
        }

        public static string VisualStudioSolutionPath
        {
            get { return _visualStudioSolutionPath; }
        }

        #endregion

        #region Methods

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            if(_mvcApplicationHostProxy != null)
                _mvcApplicationHostProxy.Dispose();
        }

        public static MvcApplicationHostProxy CreateMvcApplicationHostProxy()
        {
            return MvcApplicationHostProxyFactory.Create(TestApplicationProjectPath);
        }

        #endregion
    }
}