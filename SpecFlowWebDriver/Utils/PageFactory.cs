using SpecFlowWebDriver.Pages;

namespace SpecFlowWebDriver.Utils
{
    public static class PageFactory
    {
        private static ChromePage chromePage;
        private static WikiPage wikiPage;

        public static ChromePage GetChromePage()
        {
            if (chromePage == null)
            {
                chromePage = new ChromePage(DriverProvider.GetDriver());
            }
            return chromePage;
        }

        public static WikiPage GetWikiPage()
        {
            if (wikiPage == null)
            {
                wikiPage = new WikiPage(DriverProvider.GetDriver());
            }
            return wikiPage;
        }
    }
}
