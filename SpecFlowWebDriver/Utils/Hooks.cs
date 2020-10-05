using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.MarkupUtils;
using System;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Bindings;

namespace SpecFlowWebDriver.Utils
{
    [Binding]
    public sealed class Hooks
    {

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            Reporter.SetupExtentReports();
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            Reporter.feature = Reporter.report.CreateTest<Feature>(featureContext.FeatureInfo.Title, featureContext.FeatureInfo.Description);
        }

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            if (scenarioContext.ScenarioInfo.Tags.Contains("web")) DriverProvider.DriverType = DriverType.Web;
            if (scenarioContext.ScenarioInfo.Tags.Contains("mobile")) DriverProvider.DriverType = DriverType.Mobile;
            if (scenarioContext.ScenarioInfo.Tags.Contains("desktop")) DriverProvider.DriverType = DriverType.Desktop;
            if (scenarioContext.ScenarioInfo.Tags.Contains("nodriver")) DriverProvider.DriverType = DriverType.None;
            Reporter.scenario = Reporter.feature.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);
        }

        [BeforeStep]
        public void BeforeStep(ScenarioContext scenarioContext)
        {
            switch (scenarioContext.StepContext.StepInfo.StepDefinitionType)
            {
                case StepDefinitionType.Given:
                    Reporter.step = Reporter.scenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text);
                    break;
                case StepDefinitionType.When:
                    Reporter.step = Reporter.scenario.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text);
                    break;
                case StepDefinitionType.Then:
                    Reporter.step = Reporter.scenario.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text);
                    break;
            }
        }

        [AfterStep]
        public void AfterStep(ScenarioContext scenarioContext)
        {
            if (scenarioContext.TestError != null)
            {
                string url = string.Empty;
                string pageSource = string.Empty;
                try
                {
                    url = DriverProvider.GetDriver()?.Url;
                    pageSource = PageSourceHelper.GetPageSource();
                }
                catch (Exception) { }
                string[,] data = new string[,]
                {
                    { "Exception", $"{scenarioContext.TestError.Message}"},
                    { "StackTrace", $"{scenarioContext.TestError.StackTrace}"},
                    { "URL", $"<a href=\"{url}\">{url}</a>"},
                    { "PageSource", pageSource}
                };
                Reporter.step.Fail(MarkupHelper.CreateTable(data));
            }
            if(DriverProvider.DriverType is not DriverType.None)
                Reporter.step.Log(Status.Info, MediaEntityBuilder.CreateScreenCaptureFromPath(ScreenShotHelper.CaptureScreen()).Build());
        }

        [AfterScenario]
        public void AfterScenario()
        {
            DriverProvider.CloseDriver();
        }

        [AfterFeature]
        public static void AfterFeature()
        {
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            Reporter.report.Flush();
        }
    }
}
