using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.Web;
using System.Web.Configuration;
using HansKindberg.Web.Simulation.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HansKindberg.Web.Simulation.IntegrationTests.Hosting
{
    [TestClass]
    public class VirtualApplicationHostProxyTest
    {
        #region Methods

        [TestMethod]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        public void ExampleThatCanBeRemovedWhenMoreTestsAreImplemented()
        {
            string physicalDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
            IFileTransfer fileTransfer = new FileTransfer(new FileSystem(), physicalDirectoryPath, physicalDirectoryPath);

            // ReSharper disable PossibleNullReferenceException
            DirectoryInfoBase projectDirectory = new DirectoryInfo(physicalDirectoryPath).Parent.Parent;
            string webConfigSourcePath = Path.Combine(projectDirectory.FullName, @"Hosting\Test-configurations\VirtualApplicationHostProxyTest.ExampleThatCanBeRemovedWhenMoreTestsAreImplemented.Web.config");
            string webConfigDestinationPath = Path.Combine(physicalDirectoryPath, "Web.config");
            fileTransfer.AddFile(webConfigSourcePath, webConfigDestinationPath);
            // ReSharper restore PossibleNullReferenceException

            SimulatedVirtualPathProvider simulatedVirtualPathProvider = new SimulatedVirtualPathProvider();
            simulatedVirtualPathProvider.VirtualFiles.CreateWithDefaultWebFormContentAndAddFile("/Default.aspx");
            simulatedVirtualPathProvider.VirtualFiles.CreateAndAddDirectory("/Admin");
            simulatedVirtualPathProvider.VirtualFiles.CreateWithDefaultWebFormContentAndAddFile("/Admin/Default.aspx");

            using(VirtualApplicationHostProxy virtualApplicationHostProxy = Global.VirtualApplicationHostProxyFactory.Create(fileTransfer, simulatedVirtualPathProvider))
            {
                Assert.IsNotNull(virtualApplicationHostProxy.ApplicationHost);

                virtualApplicationHostProxy.AnyApplicationEvent += delegate(HttpApplicationEvent httpApplicationEvent)
                {
                    if(httpApplicationEvent == HttpApplicationEvent.PostRequestHandlerExecute)
                    {
                        //Assert.AreEqual(1, 2);
                    }
                };

                virtualApplicationHostProxy.Run(() =>
                {
                    RequestResult requestResult = new BrowsingSession<RequestResult>(new HttpRuntimeWrapper()).ProcessRequest("/Admin/Default.aspx");

                    if(requestResult.LastException is AssertFailedException)
                        throw requestResult.LastException;

                    Assert.IsNotNull(requestResult);
                });

                virtualApplicationHostProxy.Run(() =>
                {
                    Assert.AreEqual("Test", ConfigurationManager.AppSettings["Test"]);

                    string url = "Admin/Default.aspx/host?A=a&B=b";

                    using(StringWriter stringWriter = new StringWriter(CultureInfo.CurrentCulture))
                    {
                        new HttpRuntimeWrapper().ProcessRequest(new SimulatedWorkerRequest(url, new Dictionary<string, string>(), new Dictionary<string, string>(), new Dictionary<string, string>(), HttpVerb.Get, stringWriter));
                    }

                    RequestResult requestResult = RequestResult.Instance<RequestResult>();
                    Assert.IsNotNull(requestResult.Response);
                    Assert.AreEqual("/" + url, requestResult.Request.RawUrl);
                    // ReSharper disable PossibleNullReferenceException
                    Assert.AreEqual("http://hanskindberg.web.simulation/" + url, requestResult.Request.Url.ToString());
                    // ReSharper restore PossibleNullReferenceException

                    BrowsingSession<RequestResult> browsingSession = new BrowsingSession<RequestResult>(new HttpRuntimeWrapper());

                    url = "/Admin/Default.aspx/host?A=a&B=b";

                    requestResult = browsingSession.ProcessRequest(url);
                    Assert.IsNotNull(requestResult.Content);
                    Assert.AreEqual(url, requestResult.Request.RawUrl);
                    // ReSharper disable PossibleNullReferenceException
                    Assert.AreEqual("http://hanskindberg.web.simulation" + url, requestResult.Request.Url.ToString());
                    // ReSharper restore PossibleNullReferenceException

                    browsingSession = new BrowsingSession<RequestResult>(new HttpRuntimeWrapper());

                    url = "http://mycompany.com/Admin/Default.aspx?A=a&B=b&C=c";

                    requestResult = browsingSession.ProcessRequest(url);
                    Assert.IsNotNull(requestResult.Content);
                    // ReSharper disable PossibleNullReferenceException
                    Assert.AreEqual(url, requestResult.Request.Url.ToString());
                    // ReSharper restore PossibleNullReferenceException
                });

                virtualApplicationHostProxy.Run(browsingSession =>
                {
                    const string url = "/Admin/Default.aspx/host?A=a&B=b";
                    RequestResult requestResult = browsingSession.ProcessRequest(url);
                    Assert.AreEqual(url, requestResult.Request.RawUrl);
                });
            }
        }

        [TestMethod]
        public void Run_BrowsingSession_IfAddingAnyApplicationEventHandlerWithAssertTests_ShouldHandleTheAssertTestsByUsingTheLastErrorOfTheRequest()
        {
            using(VirtualApplicationHostProxy virtualApplicationHostProxy = Global.VirtualApplicationHostProxyFactory.Create())
            {
                virtualApplicationHostProxy.AnyApplicationEvent += delegate(HttpApplicationEvent httpApplicationEvent)
                {
                    if(httpApplicationEvent != HttpApplicationEvent.BeginRequest)
                        return;

                    // Here you should assert something in the application domain, eg. Assert.AreEqual(3, ConfigurationManager.AppSettings.Count).
                    Assert.AreEqual(1, 2);
                };

                virtualApplicationHostProxy.Run(browsingSession =>
                {
                    RequestResult requestResult = browsingSession.ProcessRequest("/");

                    Exception exception = requestResult.LastException;

                    if(exception == null)
                        Assert.Fail("There was no exception.");

                    AssertFailedException assertFailedException = exception as AssertFailedException;

                    if(assertFailedException == null)
                        Assert.Fail("The last exception is of type \"{0}\".", new object[] {exception.GetType().FullName});

                    if(assertFailedException.Message != "Assert.AreEqual failed. Expected:<1>. Actual:<2>. ")
                        Assert.Fail("The last exception message is \"{0}\".", new object[] {exception.Message});
                });
            }
        }

        [TestMethod]
        public void Run_BrowsingSession_IfSubscribingToPostRequestHandlerExecute_ShouldHandleReadingConfigurationLocationsProperly()
        {
            string physicalDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
            IFileTransfer fileTransfer = new FileTransfer(new FileSystem(), physicalDirectoryPath, physicalDirectoryPath);

            // ReSharper disable PossibleNullReferenceException
            DirectoryInfoBase projectDirectory = new DirectoryInfo(physicalDirectoryPath).Parent.Parent;
            string webConfigSourcePath = Path.Combine(projectDirectory.FullName, @"Hosting\Test-configurations\VirtualApplicationHostProxyTest.Run_BrowsingSession_ShouldHandleReadingConfigurationLocationsProperly.Web.config");
            string webConfigDestinationPath = Path.Combine(physicalDirectoryPath, "Web.config");
            fileTransfer.AddFile(webConfigSourcePath, webConfigDestinationPath);
            // ReSharper restore PossibleNullReferenceException

            SimulatedVirtualPathProvider simulatedVirtualPathProvider = new SimulatedVirtualPathProvider();
            simulatedVirtualPathProvider.VirtualFiles.CreateWithDefaultWebFormContentAndAddFile("/Default.aspx");
            simulatedVirtualPathProvider.VirtualFiles.CreateWithDefaultWebFormContentAndAddFile("/SixNamespacesByUsingAddAndRemove/Default.aspx");
            simulatedVirtualPathProvider.VirtualFiles.CreateWithDefaultWebFormContentAndAddFile("/NoNamespacesByUsingClear/Default.aspx");

            const string postRequestHandlerExecuteIsInvokedContextName = "PostRequestHandlerExecuteIsInvoked";

            using(VirtualApplicationHostProxy virtualApplicationHostProxy = Global.VirtualApplicationHostProxyFactory.Create(fileTransfer, simulatedVirtualPathProvider))
            {
                virtualApplicationHostProxy.AnyApplicationEvent += delegate(HttpApplicationEvent httpApplicationEvent)
                {
                    if(httpApplicationEvent != HttpApplicationEvent.PostRequestHandlerExecute)
                        return;

                    HttpContext.Current.Items.Add(postRequestHandlerExecuteIsInvokedContextName, 1);

                    NamespaceCollection namespaces = ((PagesSection) ConfigurationManager.GetSection("system.web/pages")).Namespaces;

                    Assert.AreEqual(3, namespaces.Count);
                    Assert.AreEqual("System", namespaces[0].Namespace);
                    Assert.AreEqual("System.CodeDom", namespaces[1].Namespace);
                    Assert.AreEqual("System.Collections", namespaces[2].Namespace);
                };

                virtualApplicationHostProxy.Run(browsingSession =>
                {
                    RequestResult requestResult = browsingSession.ProcessRequest("/Default.aspx");

                    if((int) requestResult.Context.Items[postRequestHandlerExecuteIsInvokedContextName] != 1)
                        Assert.Fail("PostRequestHandlerExecute was not invoked.");

                    if(requestResult.LastException is UnitTestAssertException)
                        throw requestResult.LastException;
                });
            }

            using(VirtualApplicationHostProxy virtualApplicationHostProxy = Global.VirtualApplicationHostProxyFactory.Create(fileTransfer, simulatedVirtualPathProvider))
            {
                virtualApplicationHostProxy.AnyApplicationEvent += delegate(HttpApplicationEvent httpApplicationEvent)
                {
                    if(httpApplicationEvent != HttpApplicationEvent.PostRequestHandlerExecute)
                        return;

                    HttpContext.Current.Items.Add(postRequestHandlerExecuteIsInvokedContextName, 1);

                    NamespaceCollection namespaces = ((PagesSection) ConfigurationManager.GetSection("system.web/pages")).Namespaces;

                    Assert.AreEqual(6, namespaces.Count);
                    Assert.AreEqual("System.CodeDom", namespaces[0].Namespace);
                    Assert.AreEqual("System.ComponentModel", namespaces[1].Namespace);
                    Assert.AreEqual("System.Configuration", namespaces[2].Namespace);
                    Assert.AreEqual("System.Deployment", namespaces[3].Namespace);
                    Assert.AreEqual("System.Diagnostics", namespaces[4].Namespace);
                    Assert.AreEqual("System.Globalization", namespaces[5].Namespace);
                };

                virtualApplicationHostProxy.Run(browsingSession =>
                {
                    RequestResult requestResult = browsingSession.ProcessRequest("/SixNamespacesByUsingAddAndRemove/Default.aspx");

                    if((int) requestResult.Context.Items[postRequestHandlerExecuteIsInvokedContextName] != 1)
                        Assert.Fail("PostRequestHandlerExecute was not invoked.");

                    if(requestResult.LastException is UnitTestAssertException)
                        throw requestResult.LastException;
                });
            }

            using(VirtualApplicationHostProxy virtualApplicationHostProxy = Global.VirtualApplicationHostProxyFactory.Create(fileTransfer, simulatedVirtualPathProvider))
            {
                virtualApplicationHostProxy.AnyApplicationEvent += delegate(HttpApplicationEvent httpApplicationEvent)
                {
                    if(httpApplicationEvent != HttpApplicationEvent.PostRequestHandlerExecute)
                        return;

                    HttpContext.Current.Items.Add(postRequestHandlerExecuteIsInvokedContextName, 1);

                    NamespaceCollection namespaces = ((PagesSection) ConfigurationManager.GetSection("system.web/pages")).Namespaces;

                    Assert.AreEqual(0, namespaces.Count);
                };

                virtualApplicationHostProxy.Run(browsingSession =>
                {
                    RequestResult requestResult = browsingSession.ProcessRequest("/NoNamespacesByUsingClear/Default.aspx");

                    if((int) requestResult.Context.Items[postRequestHandlerExecuteIsInvokedContextName] != 1)
                        Assert.Fail("PostRequestHandlerExecute was not invoked.");

                    if(requestResult.LastException is UnitTestAssertException)
                        throw requestResult.LastException;
                });
            }
        }

        [TestMethod]
        public void Run_Code_IfAddingAnyApplicationEventHandlerWithAssertTests_ShouldHandleTheAssertTestsByUsingTheLastErrorOfTheRequest()
        {
            using(VirtualApplicationHostProxy virtualApplicationHostProxy = Global.VirtualApplicationHostProxyFactory.Create())
            {
                virtualApplicationHostProxy.AnyApplicationEvent += delegate(HttpApplicationEvent httpApplicationEvent)
                {
                    if(httpApplicationEvent != HttpApplicationEvent.BeginRequest)
                        return;

                    // Here you should assert something in the application domain, eg. Assert.AreEqual(3, ConfigurationManager.AppSettings.Count).
                    Assert.AreEqual(1, 2);
                };

                virtualApplicationHostProxy.Run(() =>
                {
                    using(StringWriter output = new StringWriter(CultureInfo.CurrentCulture))
                    {
                        HttpRuntime.ProcessRequest(new SimulatedWorkerRequest("/", output));
                    }

                    RequestResult requestResult = RequestResult.Instance<RequestResult>();

                    Exception exception = requestResult.LastException;

                    if(exception == null)
                        Assert.Fail("There was no exception.");

                    AssertFailedException assertFailedException = exception as AssertFailedException;

                    if(assertFailedException == null)
                        Assert.Fail("The last exception is of type \"{0}\".", new object[] {exception.GetType().FullName});

                    if(assertFailedException.Message != "Assert.AreEqual failed. Expected:<1>. Actual:<2>. ")
                        Assert.Fail("The last exception message is \"{0}\".", new object[] {exception.Message});
                });
            }
        }

        [TestMethod]
        public void Run_Code_ShouldHandleReadingConfiguration()
        {
            string physicalDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
            IFileTransfer fileTransfer = new FileTransfer(new FileSystem(), physicalDirectoryPath, physicalDirectoryPath);

            // ReSharper disable PossibleNullReferenceException
            DirectoryInfoBase projectDirectory = new DirectoryInfo(physicalDirectoryPath).Parent.Parent;
            string webConfigSourcePath = Path.Combine(projectDirectory.FullName, @"Hosting\Test-configurations\VirtualApplicationHostProxyTest.Run_Code_ShouldHandleReadingConfiguration.Web.config");
            string webConfigDestinationPath = Path.Combine(physicalDirectoryPath, "Web.config");
            fileTransfer.AddFile(webConfigSourcePath, webConfigDestinationPath);
            // ReSharper restore PossibleNullReferenceException

            using(VirtualApplicationHostProxy virtualApplicationHostProxy = Global.VirtualApplicationHostProxyFactory.Create(fileTransfer))
            {
                virtualApplicationHostProxy.Run(() => Assert.AreEqual("TestValue", ConfigurationManager.AppSettings["Test"]));
            }
        }

        #endregion
    }
}