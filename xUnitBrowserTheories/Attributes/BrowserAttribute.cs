using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Extensions;
using Selenium;

namespace xUnitBrowserTheories
{
    public class BrowserAttribute : DataAttribute
    {
        private ISelenium Browser { get; set; }
        private string _browser { get; set; }
        public string _url { get; set; }
        
        /// <summary>
        /// Also requires a [URL] attribute, if not passing in the URL explicitly
        /// </summary>
        /// <param name="browser"></param>
        public BrowserAttribute(string browser)
        {
            _browser = browser;
        }

        public BrowserAttribute(string browser, string testStartPageUrl)
        {
            _browser = browser;
            _url = testStartPageUrl;
        }

        private SeleniumProvider CreateBrowser()
        {
            if (_url == null)
                throw new NullReferenceException(
                    "No [URL] attribute was defined, either define [URL], or pass the URL of the start page to BrowserAttribute's constructor");

            switch (_browser)
            {
                case "Internet Explorer 6":
                    Browser = new DefaultSelenium("localhost", 4444, "*iexplore", _url);
                    break;
                case "Internet Explorer 7":
                    Browser = new DefaultSelenium("localhost", 4444, "*iexplore", _url);
                    break;
                case "Internet Explorer 8":
                    Browser = new DefaultSelenium("localhost", 4444, "*iexplore", _url);
                    break;
                case "Firefox 2":
                    Browser = new DefaultSelenium("localhost", 4444, "*firefox", _url);
                    break;
                case "Firefox 3":
                    Browser = new DefaultSelenium("localhost", 4444, "*firefox", _url);
                    break;
                case "Firefox 3.5":
                    Browser = new DefaultSelenium("localhost", 4444, "*firefox", _url);
                    break;
                case "Google Chrome":
                    Browser = new DefaultSelenium("localhost", 4444, "*googlechrome", _url);
                    break;
                case "Opera":
                    Browser = new DefaultSelenium("localhost", 4444, "*opera", _url);
                    break;
            }
            return new SeleniumProvider(Browser);
        }

        public override IEnumerable<object[]> GetData(MethodInfo methodUnderTest, Type[] parameterTypes)
        {
           return new[] { new[] { CreateBrowser() } };
        }
    }
}