using System.Diagnostics.CodeAnalysis;
using System.IO;
using HansKindberg.Web.Simulation.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HansKindberg.Web.Simulation.IntegrationTests
{
    [TestClass]
    [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Global")]
    public static class Global
    {
        #region Fields

        private static readonly ApplicationHostProxy _applicationHostProxy;
        private static readonly IApplicationHostProxyFactory _applicationHostProxyFactory;
        private static readonly string _testApplicationProjectPath;
        private static readonly VirtualApplicationHostProxy _virtualApplicationHostProxy;
        private static readonly IVirtualApplicationHostProxyFactory _virtualApplicationHostProxyFactory;
        private static readonly string _visualStudioSolutionPath;

        #endregion

        #region Constructors

        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static Global()
        {
            _visualStudioSolutionPath = Path.GetFullPath("..\\..\\..");
            _testApplicationProjectPath = Path.Combine(_visualStudioSolutionPath, "HansKindberg.Web.Simulation.Application");
            _applicationHostProxyFactory = new ApplicationHostProxyFactory();
            _applicationHostProxy = _applicationHostProxyFactory.Create(_testApplicationProjectPath);
            _virtualApplicationHostProxyFactory = new VirtualApplicationHostProxyFactory();
            _virtualApplicationHostProxy = _virtualApplicationHostProxyFactory.Create();
        }

        #endregion

        #region Properties

        public static ApplicationHostProxy ApplicationHostProxy
        {
            get { return _applicationHostProxy; }
        }

        public static IApplicationHostProxyFactory ApplicationHostProxyFactory
        {
            get { return _applicationHostProxyFactory; }
        }

        public static string TestApplicationProjectPath
        {
            get { return _testApplicationProjectPath; }
        }

        public static VirtualApplicationHostProxy VirtualApplicationHostProxy
        {
            get { return _virtualApplicationHostProxy; }
        }

        public static IVirtualApplicationHostProxyFactory VirtualApplicationHostProxyFactory
        {
            get { return _virtualApplicationHostProxyFactory; }
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
            if(_applicationHostProxy != null)
                _applicationHostProxy.Dispose();

            if(_virtualApplicationHostProxy != null)
                _virtualApplicationHostProxy.Dispose();
        }

        public static ApplicationHostProxy CreateApplicationHostProxy()
        {
            return ApplicationHostProxyFactory.Create(TestApplicationProjectPath);
        }

        #endregion
    }
}