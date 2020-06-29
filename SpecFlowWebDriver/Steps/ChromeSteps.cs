using NUnit.Framework;
using SpecFlowWebDriver.Pages;
using SpecFlowWebDriver.Utils;
using TechTalk.SpecFlow;

namespace SpecFlowWebDriver.Steps
{
    [Binding, Parallelizable]
    public class ChromeSteps
    {
        ChromePage chromePage = new ChromePage(DriverProvider.GetDriver());
        [When(@"I google for a (.*)")]
        public void WhenISearchForA(string definition)
        {
            chromePage.GoogleThing(definition);
        }
        [Then(@"The google of (.*) is displayed")]
        public void ThenTheDefinitionOfIsDisplayed(string definition)
        {
            Assert.AreEqual(chromePage.GoogleInput.Text, definition);
        }
    }
}
