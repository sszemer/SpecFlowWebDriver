using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.MarkupUtils;
using NUnit.Framework;
using SpecFlowWebDriver.Utis;
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
            EnvironmentHelper.EnvironmentType = EnvironmentType.LAMBDA_TEST;
            Reporter.SetupExtentReports();
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            Reporter.Feature = Reporter.Report.CreateTest<Feature>(featureContext.FeatureInfo.Title, featureContext.FeatureInfo.Description);
        }

        [BeforeScenario]
        public static void BeforeScenario(ScenarioContext scenarioContext)
        {
            if (scenarioContext.ScenarioInfo.Tags.Contains("web")) scenarioContext.Set<DriverType>(DriverType.Web);
            if (scenarioContext.ScenarioInfo.Tags.Contains("mobile")) scenarioContext.Set<DriverType>(DriverType.Mobile);
            if (scenarioContext.ScenarioInfo.Tags.Contains("desktop")) scenarioContext.Set<DriverType>(DriverType.Desktop);
            if (scenarioContext.ScenarioInfo.Tags.Contains("nodriver")) scenarioContext.Set<DriverType>(DriverType.None);
            Reporter.Scenario = Reporter.Feature.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);
            try
            {
                DriverProvider.InitDriver(scenarioContext);
            }
            catch (Exception e)
            {
                Reporter.Scenario.CreateNode<Given>($"scenario failed: {e.Message}").Fail("").Log(Status.Error, e);
                Assert.Ignore($"scenario failed: {e.Message}");
            }
        }

        [BeforeStep]
        public static void BeforeStep(ScenarioContext scenarioContext)
        {
            switch (scenarioContext.StepContext.StepInfo.StepDefinitionType)
            {
                case StepDefinitionType.Given:
                    Reporter.Step = Reporter.Scenario.CreateNode<Given>(scenarioContext.StepContext.StepInfo.Text);
                    break;
                case StepDefinitionType.When:
                    Reporter.Step = Reporter.Scenario.CreateNode<When>(scenarioContext.StepContext.StepInfo.Text);
                    break;
                case StepDefinitionType.Then:
                    Reporter.Step = Reporter.Scenario.CreateNode<Then>(scenarioContext.StepContext.StepInfo.Text);
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
                    { "PageSource", $"<a href=\"{pageSource}\">{pageSource}</a>"}
                };
                Reporter.Step.Fail(MarkupHelper.CreateTable(data));
            }
            if (scenarioContext.Get<DriverType>() is not DriverType.None)
                Reporter.Step.Log(Status.Info, MediaEntityBuilder.CreateScreenCaptureFromPath(DriverProvider.GetScreenshot(scenarioContext)).Build());
        }

        [AfterScenario]
        public static void AfterScenario(ScenarioContext scenarioContext)
        {
            DriverProvider.CloseDriver(scenarioContext);
            Reporter.Scenario.AssignCategory("All_tests");
            scenarioContext.ScenarioInfo.Tags.ToList().ForEach(tag => Reporter.Scenario.AssignCategory(tag));
            Reporter.Report.Flush();
        }

        [AfterFeature]
        public static void AfterFeature()
        {
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            Reporter.Report.Flush();
        }
    }
}
