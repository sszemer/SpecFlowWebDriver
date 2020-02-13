using System;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SpecFlowWebDriver
{
    [Binding]
    public class WikiSearchSteps
    {

        static IWebDriver driver;

        [BeforeFeature]
        static void setUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        [Given(@"Wiki page is open")]
        public void GivenWikiPageIsOpen()
        {
            driver.Url = "https://pl.wikipedia.org/";
        }
        
        [When(@"I search for a (.*)")]
        public void WhenISearchForA(string p0)
        {
            IWebElement e = driver.FindElement(By.Id("searchInput"));
            e.SendKeys(p0);
            e.Submit();
        }
        
        [Then(@"The definition of (.*) is displayed")]
        public void ThenTheDefinitionOfIsDisplayed(string p0)
        {
            driver.Title.Equals(p0);
        }

        [AfterFeature]
        static void tearDown()
        {
            driver.Close();
        }
    }
}
