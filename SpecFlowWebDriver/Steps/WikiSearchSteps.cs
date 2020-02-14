using TechTalk.SpecFlow;
using NUnit.Framework;
using SpecFlowWebDriver.Pages;
using OpenQA.Selenium.Chrome;
using SpecFlowWebDriver.Utils;

namespace SpecFlowWebDriver
{
    [Binding, Parallelizable]
    public class WikiSearchSteps
    {
        WikiPage wikiPage = new WikiPage(DriverProvider.GetDriver());

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
    }
}
