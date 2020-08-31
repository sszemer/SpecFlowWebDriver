using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace SpecFlowWebDriver.Pages
{
    public class WikiPage : CommonPage
    {
        public WikiPage(RemoteWebDriver driver) : base(driver) { }
        public IWebElement SearchInput => driver.FindElement(By.Id("searchInput"));
        public IWebElement ArticleName => driver.FindElement(By.Id("firstHeading"));
        public void Go() => driver.Navigate().GoToUrl("https://pl.wikipedia.org/");
    }
}
