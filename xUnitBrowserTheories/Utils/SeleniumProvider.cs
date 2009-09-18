using Selenium;

namespace xUnitBrowserTheories
{
    public class SeleniumProvider
    {
        private ISelenium Browser;

        public SeleniumProvider(ISelenium selenium)
        {
            Browser = selenium;
        }

        public ISelenium GetBrowser()
        {
            Browser.Start();
            return Browser;
        }
    }
}
