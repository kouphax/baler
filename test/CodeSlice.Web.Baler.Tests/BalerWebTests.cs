using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CassiniDev;
using System.Net;
using NUnit.Framework;
using WatiN.Core;

namespace CodeSlice.Web.Baler.Tests
{
    [TestFixture(Ignore=true, IgnoreReason="Running this with ContinuiousTests is a big overhead for what I want to do")]
    [RequiresSTA]
    public class BalerWebTests
    {
        CassiniDevServer _server;
        string _baseUrl;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _server = new CassiniDevServer();
            _server.StartServer(@"..\..\..\CodeSlice.Web.Baler.Web.Tests");
            _baseUrl = _server.NormalizeUrl("Default.aspx");
        }

        [Test]
        public void MyTest()
        {
            using (Browser browser = new IE(_baseUrl))
            {
                Div result = browser.GetFirstByClass<Div>("runner");
                Assert.That(result.ClassName, Is.StringContaining("passed"));
            }
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            _server.StopServer();
        }
    }

    public static class BrowserExtensions
    {
        public static TElement GetFirstByClass<TElement>(this Browser browser, string className) where TElement : Element
        {
            return browser.ElementsOfType<TElement>().First(e => e.HasClass(className));
        }

        public static bool HasClass<TElement>(this TElement element, string className) where TElement : Element
        {
            return element.ClassName.Split(' ').Any(c => c == className);
        }
    }
}
