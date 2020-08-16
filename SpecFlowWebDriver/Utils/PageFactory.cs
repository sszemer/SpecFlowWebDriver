using OpenQA.Selenium.Remote;
using SpecFlowWebDriver.Pages;

namespace SpecFlowWebDriver.Utils
{
    public static class PageFactory
    {
        private static ChromePage chromePage;
        private static WikiPage wikiPage;
        private static readonly RemoteWebDriver remoteWebDriver = DriverProvider.GetDriver();

        public static ChromePage GetChromePage()
        {
            if (chromePage == null)
            {
                chromePage = new ChromePage(remoteWebDriver);
            }
            return chromePage;
        }

        public static WikiPage GetWikiPage()
        {
            if (wikiPage == null)
            {
                wikiPage = new WikiPage(remoteWebDriver);
            }
            return wikiPage;
        }
    }
}
