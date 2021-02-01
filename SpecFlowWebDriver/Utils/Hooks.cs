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
        private static ReportPOCO reportPOCO;

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            reportPOCO = new ReportPOCO();
            EnvironmentHelper.EnvironmentType = EnvironmentType.LAMBDA_TEST;
            Reporter.SetupExtentReports();
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            featureContext.Set<FeaturePOCO>(new FeaturePOCO(featureContext.FeatureInfo.Title, featureContext.FeatureInfo.Description));
        }

        [BeforeScenario]
        public static void BeforeScenario(ScenarioContext scenarioContext)
        {
            if (scenarioContext.ScenarioInfo.Tags.Contains("web")) scenarioContext.Set<DriverType>(DriverType.Web);
            if (scenarioContext.ScenarioInfo.Tags.Contains("mobile")) scenarioContext.Set<DriverType>(DriverType.Mobile);
            if (scenarioContext.ScenarioInfo.Tags.Contains("desktop")) scenarioContext.Set<DriverType>(DriverType.Desktop);
            if (scenarioContext.ScenarioInfo.Tags.Contains("nodriver")) scenarioContext.Set<DriverType>(DriverType.None);
            scenarioContext.Set(new ScenarioPOCO(scenarioContext.ScenarioInfo.Title));
            try
            {
                DriverProvider.InitDriver(scenarioContext);
            }
            catch (Exception e)
            {
                scenarioContext.Get<ScenarioPOCO>().Steps.Add(new StepPOCO($"scenario failed: {e.Message}", StepDefinitionType.Given, Status.Error));
                scenarioContext.Get<ScenarioPOCO>().Steps.Last().Exception = e;
                Assert.Ignore($"scenario failed: {e.Message}");
            }
        }

        [BeforeStep]
        public static void BeforeStep(ScenarioContext scenarioContext)
        {
            switch (scenarioContext.StepContext.StepInfo.StepDefinitionType)
            {
                case StepDefinitionType.Given:
                    scenarioContext.Get<ScenarioPOCO>().Steps.Add(new StepPOCO(scenarioContext.StepContext.StepInfo.Text, StepDefinitionType.Given));
                    break;
                case StepDefinitionType.When:
                    scenarioContext.Get<ScenarioPOCO>().Steps.Add(new StepPOCO(scenarioContext.StepContext.StepInfo.Text, StepDefinitionType.When));
                    break;
                case StepDefinitionType.Then:
                    scenarioContext.Get<ScenarioPOCO>().Steps.Add(new StepPOCO(scenarioContext.StepContext.StepInfo.Text, StepDefinitionType.Then));
                    break;
            }
        }

        [AfterStep]
        public static void AfterStep(ScenarioContext scenarioContext)
        {
            scenarioContext.Get<ScenarioPOCO>().Steps.Last().Screenshot = DriverProvider.GetScreenshot(scenarioContext);
            scenarioContext.Get<ScenarioPOCO>().Steps.Last().StepStatus = Status.Pass;
            if (scenarioContext.TestError != null)
            {
                scenarioContext.Get<ScenarioPOCO>().Steps.Last().PageSource = DriverProvider.GetPageSource(scenarioContext);
                scenarioContext.Get<ScenarioPOCO>().Steps.Last().URL = DriverProvider.GetUrl(scenarioContext);
                scenarioContext.Get<ScenarioPOCO>().Steps.Last().StepStatus = Status.Error;
                scenarioContext.Get<ScenarioPOCO>().Steps.Last().Exception = scenarioContext.TestError;
            }
        }

        [AfterScenario]
        public static void AfterScenario(ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            scenarioContext.ScenarioInfo.Tags.ToList().ForEach(tag => scenarioContext.Get<ScenarioPOCO>().Categories.Add(tag));
            scenarioContext.Get<ScenarioPOCO>().Categories.Add("All_tests");
            featureContext.Get<FeaturePOCO>().Scenarios.Add(scenarioContext.Get<ScenarioPOCO>());
            DriverProvider.CloseDriver(scenarioContext);
        }

        [AfterFeature]
        public static void AfterFeature(FeatureContext featureContext)
        {
            reportPOCO.Features.Add(featureContext.Get<FeaturePOCO>());
            foreach (var feature in reportPOCO.Features)
            {
                Reporter.Feature = Reporter.Report.CreateTest<Feature>(feature.Title, feature.Description);
                foreach (var scenario in feature.Scenarios)
                {
                    Reporter.Scenario = Reporter.Feature.CreateNode<Scenario>(scenario.Title);
                    scenario.Categories.Sort();
                    scenario.Categories.ForEach(category => Reporter.Scenario.AssignCategory(category));
                    foreach (var step in scenario.Steps)
                    {
                        switch (step.StepType)
                        {
                            case StepDefinitionType.Given:
                                Reporter.Step = Reporter.Scenario.CreateNode<Given>(step.Title);
                                break;
                            case StepDefinitionType.When:
                                Reporter.Step = Reporter.Scenario.CreateNode<When>(step.Title);
                                break;
                            case StepDefinitionType.Then:
                                Reporter.Step = Reporter.Scenario.CreateNode<Then>(step.Title);
                                break;
                        }
                        if (step.StepStatus is Status.Error)
                        {
                            string[,] data = new string[4, 2]
                            {
                                { "Exception", $"{step.Exception.Message}"},
                                { "StackTrace", $"{step.Exception.StackTrace}"},
                                { "URL", $"<a href=\"{step.URL}\">{step.URL}</a>"},
                                { "PageSource", $"<a href=\"{step.PageSource}\">{step.PageSource}</a>"}
                            };
                            Reporter.Step.Fail(MarkupHelper.CreateTable(data));
                        }
                        if (step.Screenshot != null) Reporter.Step.Log(Status.Info, MediaEntityBuilder.CreateScreenCaptureFromPath(step.Screenshot).Build());
                    }
                }
            }
            reportPOCO = new ReportPOCO();
            Reporter.Report.Flush();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            Reporter.Report.Flush();
            NLog.LogManager.Shutdown();
        }
    }
}
