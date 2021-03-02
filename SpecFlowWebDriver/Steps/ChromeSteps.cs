using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Remote;
using SpecFlowWebDriver.Pages;
using TechTalk.SpecFlow;

namespace SpecFlowWebDriver.Steps
{
    [Binding, Parallelizable]
    public class ChromeSteps
    {
        private readonly ChromePage chromePage;
        private readonly ScenarioContext scenarioContext;

        public ChromeSteps(ScenarioContext scenarioContext)
        {
            chromePage = new ChromePage(scenarioContext.Get<AppiumDriver<AppiumWebElement>>("driver"));
            this.scenarioContext = scenarioContext;
        }

        [When(@"I google for a (.*)")]
        public void WhenISearchForA(string definition)
        {
            chromePage.GoogleThing(definition);
        }
        [Then(@"The google of (.*) is displayed")]
        public void ThenTheDefinitionOfIsDisplayed(string definition)
        {
            Assert.IsTrue(chromePage.GoogleInput.Text.StartsWith($"google.com/search?q={definition}"));
        }
    }
}
