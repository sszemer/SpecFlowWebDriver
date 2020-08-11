using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using System.IO;
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
            DriverProvider.GetDriver();
            Reporter.SetupExtentHtmlReporter();
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
            var pageSource = DriverProvider.GetDriver().PageSource;
            var path = "c:\\temp\\a.html";
            File.WriteAllText(path, pageSource);
            if (scenarioContext.TestError != null)
            {
                Reporter.step.Fail($"{scenarioContext.TestError.Message}<br/><a href=\"{path}\">Page source</a>");
                //Reporter.step.Log(Status.Info, "<br/><a href=\""+path+"\">Page source</a>");
            }
            Reporter.step.Log(Status.Info, MediaEntityBuilder.CreateScreenCaptureFromPath(ScreenShotHelpers.CaptureScreen()).Build());
        }

        [AfterScenario]
        public void AfterScenario()
        {
        }

        [AfterFeature]
        public static void AfterFeature()
        {
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            DriverProvider.CloseDriver();
            Reporter.report.Flush();
        }
    }
}
