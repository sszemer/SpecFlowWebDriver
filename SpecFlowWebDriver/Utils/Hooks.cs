using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.MarkupUtils;
using OpenQA.Selenium.Remote;
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
        public static void BeforeScenario(ScenarioContext scenarioContext)
        {
            if (scenarioContext.ScenarioInfo.Tags.Contains("web")) DriverProvider.DriverType = DriverType.Web;
            if (scenarioContext.ScenarioInfo.Tags.Contains("mobile")) DriverProvider.DriverType = DriverType.Mobile;
            if (scenarioContext.ScenarioInfo.Tags.Contains("desktop")) DriverProvider.DriverType = DriverType.Desktop;
            if (scenarioContext.ScenarioInfo.Tags.Contains("nodriver")) DriverProvider.DriverType = DriverType.None;
            Reporter.scenario = Reporter.feature.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);
            scenarioContext.Set<RemoteWebDriver>(DriverProvider.InitDriver(), "driver");
        }

        [BeforeStep]
        public static void BeforeStep(ScenarioContext scenarioContext)
        {
            switch (scenarioContext.StepContext.StepInfo.StepDefinitionType)
            {
                case StepDefinitionType.Given:
                    Reporter.step = Reporter.scenario.CreateNode<Given>(scenarioContext.StepContext.StepInfo.Text);
                    break;
                case StepDefinitionType.When:
                    Reporter.step = Reporter.scenario.CreateNode<When>(scenarioContext.StepContext.StepInfo.Text);
                    break;
                case StepDefinitionType.Then:
                    Reporter.step = Reporter.scenario.CreateNode<Then>(scenarioContext.StepContext.StepInfo.Text);
                    break;
            }
        }

        [AfterStep]
        public static void AfterStep(ScenarioContext scenarioContext)
        {
            if (scenarioContext.TestError != null)
            {
                string url = DriverProvider.GetUrl(scenarioContext);
                string pageSource = DriverProvider.GetPageSource(scenarioContext);
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
            Reporter.step.Log(Status.Info, MediaEntityBuilder.CreateScreenCaptureFromPath(DriverProvider.GetScreenshot(scenarioContext)).Build());
        }

        [AfterScenario]
        public static void AfterScenario(ScenarioContext scenarioContext)
        {
            DriverProvider.CloseDriver(scenarioContext);
            Reporter.scenario.AssignCategory("All_tests");
            scenarioContext.ScenarioInfo.Tags.ToList().ForEach(tag => Reporter.scenario.AssignCategory(tag));
            Reporter.report.Flush();
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
