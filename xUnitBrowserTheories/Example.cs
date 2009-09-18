using System;
using Selenium;
using Xunit;

namespace xUnitBrowserTheories
{
    public class Example : IDisposable
    {
        private ISelenium Browser;

        [BrowserTheory]
        [URL("http://www.google.co.uk")]
        [URL("http://www.google.com")]
        [Browser(Browsers.InternetExplorer6)]
        [Browser(Browsers.InternetExplorer7, "http://www.google.com")]
        [Browser(Browsers.InternetExplorer8)]
        [Browser(Browsers.Firefox2)]
        [Browser(Browsers.Firefox3)]
        [Browser(Browsers.Firefox3_5)]
        [Browser(Browsers.GoogleChrome)]
        [Browser(Browsers.Opera)]
        public void Google_For_SimpleTalk(SeleniumProvider seleniumProvider)
        {
            Browser = seleniumProvider.GetBrowser();

            Browser.Open("/");
            Browser.Type("q", "Simple Talk");
            Browser.Click("btnG");

            Browser.WaitForPageToLoad("3000");

            Assert.True(Browser.IsTextPresent("www.simple-talk.com"));
        }

        [BrowserTheory]
        [URL("http://SiteUnderTest/Account/Login.aspx")]
        [Browser(Browsers.GoogleChrome)]
        public void Login_Works_With_Valid_Credentials() { }

        [BrowserTheory]
        [URL("http://SiteUnderTest/Account/Register.aspx")]
        [Browser(Browsers.Firefox3_5)]
        public void Signup_Rejects_Invalid_Email_Addresses() { }


        public void Dispose()
        {
            Browser.Stop();
        }
    }
}
