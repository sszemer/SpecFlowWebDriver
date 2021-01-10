using NUnit.Framework;
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
            chromePage = new ChromePage(scenarioContext.Get<RemoteWebDriver>("driver"));
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
            Assert.AreEqual(definition, chromePage.GoogleInput.Text);
        }
    }
}
