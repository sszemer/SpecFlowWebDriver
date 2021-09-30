using TechTalk.SpecFlow;
using NUnit.Framework;
using SpecFlowWebDriver.Pages;
using OpenQA.Selenium.Remote;
using System;

namespace SpecFlowWebDriver
{
    [Binding, Parallelizable]
    public class WikiSearchSteps
    {
        private readonly WikiPage wikiPage;
        private readonly ScenarioContext scenarioContext;

        public WikiSearchSteps(ScenarioContext scenarioContext)
        {
            wikiPage = new WikiPage(scenarioContext.Get<RemoteWebDriver>("driver"));
            this.scenarioContext = scenarioContext;
        }

        [Given(@"Wiki page is open")]
        public void GivenWikiPageIsOpen()
        {
            wikiPage.Go();
        }
        
        [When(@"I search for a (.*)")]
        public void WhenISearchForA(string definition)
        {
            wikiPage.SearchInput.SendKeys(definition);
            wikiPage.SearchInput.Submit();
        }
        
        [Then(@"The definition of (.*) is displayed")]
        public void ThenTheDefinitionOfIsDisplayed(string definition)
        {
            Assert.True(wikiPage.ArticleName.Text.ToLower().Contains(definition));
        }

        [Then(@"'(.*)' cookie value is today")]
        public void ThenCookieValueIs(string cookieName)
        {
            var expectedValue = DateTime.Now.ToString("dd-MMM-yyyy");
            Assert.AreEqual(scenarioContext.Get<RemoteWebDriver>("driver").Manage().Cookies.GetCookieNamed(cookieName).Value, expectedValue);
        }

        [Then(@"'(.*)' localStorage item value is '(.*)'")]
        public void ThenLocalStorageItemValueIs(string itemName, string expected)
        {
            var actual = scenarioContext.Get<RemoteWebDriver>("driver").ExecuteScript($"return window.localStorage.getItem('{itemName}');")?.ToString();
            Assert.AreEqual(expected, actual);
        }

        [Then(@"'(.*)' sessionStorage item value is '(.*)'")]
        public void ThenSessionStorageItemValueIs(string itemName, string expected)
        {
            var actual = scenarioContext.Get<RemoteWebDriver>("driver").ExecuteScript($"return window.sessionStorage.getItem('{itemName}');")?.ToString();
            Assert.AreEqual(expected, actual);
        }
    }
}
