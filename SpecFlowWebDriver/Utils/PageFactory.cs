using OpenQA.Selenium.Remote;
using SpecFlowWebDriver.Pages;

namespace SpecFlowWebDriver.Utils
{
    public static class PageFactory
    {
        private static ChromePage chromePage;
        private static WikiPage wikiPage;
        private static readonly RemoteWebDriver remoteWebDriver = DriverProvider.GetDriver();

        public static ChromePage ChromePage => chromePage ??= new ChromePage(remoteWebDriver);
        public static WikiPage WikiPage => wikiPage ??= new WikiPage(remoteWebDriver);
    }
}
