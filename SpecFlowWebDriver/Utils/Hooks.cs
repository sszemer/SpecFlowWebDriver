using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.MarkupUtils;
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
            Reporter.scenario = Reporter.feature.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);
        }

        [BeforeStep]
        public void BeforeStep(ScenarioContext scenarioContext)
        {
            switch(scenarioContext.StepContext.StepInfo.StepDefinitionType)
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
                string[,] data = new string[,]
                {
                    { "Exception", $"{scenarioContext.TestError.Message}"},
                    { "StackTrace", $"{scenarioContext.TestError.StackTrace}"},
                    { "URL", $"<a href=\"{DriverProvider.GetDriver().Url}\">{DriverProvider.GetDriver().Url}</a>"},
                    { "PageSource", PageSourceHelper.GetPageSource()}
                };
                Reporter.step.Fail(MarkupHelper.CreateTable(data));
            }
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
