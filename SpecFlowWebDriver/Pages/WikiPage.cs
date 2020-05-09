using OpenQA.Selenium;

namespace SpecFlowWebDriver.Pages
{
    class WikiPage
    {
        IWebDriver driver;

        public WikiPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public IWebElement SearchInput => driver.FindElement(By.Id("searchInput"));

        public IWebElement ArticleName => driver.FindElement(By.Id("firstHeading"));

        public void Go()
        {
            driver.Navigate().GoToUrl("https://pl.wikipedia.org/");
        }

    }
}
