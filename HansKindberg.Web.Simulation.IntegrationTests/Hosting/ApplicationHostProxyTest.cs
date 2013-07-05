using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HansKindberg.Web.Simulation.IntegrationTests.Hosting
{
    [TestClass]
    public class ApplicationHostProxyTest
    {
        #region Methods

        [TestMethod]
        public void Run_BrowsingSession_ShouldHandleContent()
        {
            Global.ApplicationHostProxy.Run(browsingSession =>
            {
                RequestResult requestResult = browsingSession.ProcessRequest("/Default.aspx");

                Assert.IsTrue(requestResult.Content.Contains("<h2>Welcome to the sample MVP application for integration testing!</h2>"));
            });
        }

        [TestMethod]
        public void Run_BrowsingSession_ShouldHandleReadingCookies()
        {
            Global.ApplicationHostProxy.Run(browsingSession =>
            {
                const string url = "/Views/Cookie/CookieView.aspx";

                browsingSession.ProcessRequest(url);

                // ReSharper disable PossibleNullReferenceException
                Assert.AreEqual("FirstCookieValue", browsingSession.Cookies["FirstCookie"].Value);
                Assert.AreEqual("SecondCookieValue", browsingSession.Cookies["SecondCookie"].Value);
                Assert.AreEqual("ThirdCookieValue", browsingSession.Cookies["ThirdCookie"].Value);
                // ReSharper restore PossibleNullReferenceException
            });
        }

        [TestMethod]
        public void Run_BrowsingSession_ShouldHandleReadingSessions()
        {
            Global.ApplicationHostProxy.Run(browsingSession =>
            {
                const string url = "/Views/Session/SessionView.aspx";

                browsingSession.ProcessRequest(url);

                Assert.AreEqual(string.Empty, browsingSession.Session["FirstSession"]);
                Assert.AreEqual(10, browsingSession.Session["SecondSession"]);
                Assert.AreEqual(true, browsingSession.Session["ThirdSession"]);
                Assert.AreEqual(1, browsingSession.Session["IncrementSession"]);

                // Session values persist within a browsingSession
                browsingSession.ProcessRequest(url);

                Assert.AreEqual(string.Empty, browsingSession.Session["FirstSession"]);
                Assert.AreEqual(10, browsingSession.Session["SecondSession"]);
                Assert.AreEqual(true, browsingSession.Session["ThirdSession"]);
                Assert.AreEqual(2, browsingSession.Session["IncrementSession"]);

                browsingSession.ProcessRequest(url);

                Assert.AreEqual(string.Empty, browsingSession.Session["FirstSession"]);
                Assert.AreEqual(10, browsingSession.Session["SecondSession"]);
                Assert.AreEqual(true, browsingSession.Session["ThirdSession"]);
                Assert.AreEqual(3, browsingSession.Session["IncrementSession"]);
            });
        }

        [TestMethod]
        public void Run_BrowsingSession_ShouldHandleSettingCookies()
        {
            Global.ApplicationHostProxy.Run(browsingSession =>
            {
                const string url = "/Views/Cookie/CookieView.aspx";

                browsingSession.ProcessRequest(url);

                // ReSharper disable PossibleNullReferenceException
                Assert.AreEqual("FirstCookieValue", browsingSession.Cookies["FirstCookie"].Value);
                Assert.AreEqual("SecondCookieValue", browsingSession.Cookies["SecondCookie"].Value);
                Assert.AreEqual("ThirdCookieValue", browsingSession.Cookies["ThirdCookie"].Value);

                browsingSession.Cookies["FirstCookie"].Value = "NewFirstCookieValue";
                browsingSession.Cookies["SecondCookie"].Value = "NewSecondCookieValue";
                browsingSession.Cookies["ThirdCookie"].Value = "NewThirdCookieValue";

                browsingSession.ProcessRequest(url + "?Overwrite=false");

                Assert.AreEqual("NewFirstCookieValue", browsingSession.Cookies["FirstCookie"].Value);
                Assert.AreEqual("NewSecondCookieValue", browsingSession.Cookies["SecondCookie"].Value);
                Assert.AreEqual("NewThirdCookieValue", browsingSession.Cookies["ThirdCookie"].Value);
                // ReSharper restore PossibleNullReferenceException
            });
        }

        [TestMethod]
        public void Run_BrowsingSession_ShouldHandleSettingSessions()
        {
            Global.ApplicationHostProxy.Run(browsingSession =>
            {
                const string url = "/Views/Session/SessionView.aspx";
                browsingSession.ProcessRequest(url);

                Assert.AreEqual(string.Empty, browsingSession.Session["FirstSession"]);
                Assert.AreEqual(10, browsingSession.Session["SecondSession"]);
                Assert.AreEqual(true, browsingSession.Session["ThirdSession"]);
                Assert.AreEqual(1, browsingSession.Session["IncrementSession"]);

                browsingSession.Session["FirstSession"] = new Uri("http://localhost/");
                browsingSession.Session["SecondSession"] = true;
                browsingSession.Session["ThirdSession"] = 100;
                browsingSession.Session["IncrementSession"] = 100;

                browsingSession.ProcessRequest(url);

                Assert.AreEqual("http://localhost/", browsingSession.Session["FirstSession"].ToString());
                Assert.AreEqual(true, browsingSession.Session["SecondSession"]);
                Assert.AreEqual(100, browsingSession.Session["ThirdSession"]);
                Assert.AreEqual(101, browsingSession.Session["IncrementSession"]);
            });
        }

        #endregion

        //[TestMethod]
        //public void Run_BrowsingSession_ShouldHandleALogOn()
        //{
        //    const string urlThatNeedsAuthorization = "/Views/CurrentUser/CurrentUserView.aspx";
        //    Global.ApplicationHostProxy.Run(browsingSession =>
        //    {
        //        RequestResult requestResult = browsingSession.ProcessRequest(urlThatNeedsAuthorization);
        //        string redirectLocation = requestResult.Response.RedirectLocation;
        //        Assert.IsTrue(redirectLocation.StartsWith("/Views/LogOn/LogOnView.aspx", StringComparison.Ordinal));
        //        string content = browsingSession.ProcessRequest(redirectLocation).Content;
        //        string antiForgeryToken = content.ExtractAntiForgeryToken();
        //        // See the constructor for HansKindberg.Web.Simulation.Applications.Logic.Fakes.Web.Security.SimplePrincipalRepository for available users.
        //        requestResult = browsingSession.ProcessRequest(redirectLocation, HttpVerb.Post, new Dictionary<string, string>
        //            {
        //                {"UserName", "User"},
        //                {"Password", "password"},
        //                {"__RequestVerificationToken", antiForgeryToken}
        //            });
        //        redirectLocation = requestResult.Response.RedirectLocation;
        //        Assert.AreEqual(urlThatNeedsAuthorization, redirectLocation);
        //        requestResult = browsingSession.ProcessRequest(urlThatNeedsAuthorization);
        //        Assert.AreEqual("Hello, you're logged in as User", requestResult.Content);
        //    });
        //}
    }
}