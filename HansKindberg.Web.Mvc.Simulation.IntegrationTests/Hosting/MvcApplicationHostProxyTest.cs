using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HansKindberg.Web.Mvc.Simulation.Extensions;
using HansKindberg.Web.Simulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HansKindberg.Web.Mvc.Simulation.IntegrationTests.Hosting
{
    [TestClass]
    public class MvcApplicationHostProxyTest
    {
        #region Methods

        [TestMethod]
        public void Run_BrowsingSession_ShouldHandleALogOn()
        {
            const string urlThatNeedsAuthorization = "/CurrentUser/WhoAreYou";

            Global.MvcApplicationHostProxy.Run(browsingSession =>
            {
                RequestResult requestResult = browsingSession.ProcessRequest(urlThatNeedsAuthorization);
                string redirectLocation = requestResult.Response.RedirectLocation;
                Assert.IsTrue(redirectLocation.StartsWith("/Account/LogOn", StringComparison.Ordinal));

                string content = browsingSession.ProcessRequest(redirectLocation).Content;
                string antiForgeryToken = content.ExtractAntiForgeryToken();

                // See the constructor for HansKindberg.Web.Simulation.Applications.Logic.Fakes.Web.Security.SimplePrincipalRepository for available users.
                requestResult = browsingSession.ProcessRequest(redirectLocation, HttpVerb.Post, new Dictionary<string, string>
                    {
                        {"UserName", "User"},
                        {"Password", "password"},
                        {"__RequestVerificationToken", antiForgeryToken}
                    });

                redirectLocation = requestResult.Response.RedirectLocation;
                Assert.AreEqual(urlThatNeedsAuthorization, redirectLocation);

                requestResult = browsingSession.ProcessRequest(urlThatNeedsAuthorization);
                Assert.AreEqual("Hello, you're logged in as User", requestResult.Content);
            });
        }

        [TestMethod]
        public void Run_BrowsingSession_ShouldHandleReadingCookies()
        {
            Global.MvcApplicationHostProxy.Run(browsingSession =>
            {
                const string url = "/Cookie/GetCookies";

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
            Global.MvcApplicationHostProxy.Run(browsingSession =>
            {
                const string url = "/Session/GetSessions";
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
            Global.MvcApplicationHostProxy.Run(browsingSession =>
            {
                const string url = "/Cookie/GetCookies";
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
            Global.MvcApplicationHostProxy.Run(browsingSession =>
            {
                const string url = "/Session/GetSessions";
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

        [TestMethod]
        public void Run_BrowsingSession_ShouldHandleViewResults()
        {
            Global.MvcApplicationHostProxy.Run(browsingSession =>
            {
                RequestResult requestResult = browsingSession.ProcessRequest("");

                ViewResult viewResult = (ViewResult) requestResult.ActionExecutedContext.Result;
                Assert.AreEqual("Index", viewResult.ViewName);
                Assert.AreEqual("Welcome to the sample MVC application for integration testing!", viewResult.ViewData["Message"]);

                Assert.IsTrue(requestResult.Content.Contains("<!DOCTYPE html"));
            });
        }

        #endregion
    }
}