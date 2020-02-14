using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using NUnit.Framework;
using TechTalk.SpecFlow;

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
        public static void BeforeFeature()
        {
            Reporter.feature = Reporter.report.CreateTest<Feature>(FeatureContext.Current.FeatureInfo.Title, FeatureContext.Current.FeatureInfo.Description);
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            Reporter.scenario = Reporter.feature.CreateNode<Scenario>(ScenarioContext.Current.ScenarioInfo.Title);
        }

        [BeforeStep]
        public void BeforeStep()
        {
        }
        
        [AfterStep]
        public void AfterStep()
        {
            var stepType = ScenarioContext.Current.StepContext.StepInfo.StepDefinitionType.ToString();
            var testError = ScenarioContext.Current.TestError;

            if (testError == null)
            {
                if (stepType == "Given")
                    Reporter.feature.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text);
                else if (stepType == "When")
                    Reporter.feature.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text);
                else if (stepType == "Then")
                    Reporter.feature.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text);
                else if (stepType == "And")
                    Reporter.feature.CreateNode<And>(ScenarioStepContext.Current.StepInfo.Text);
            }
            else if (testError != null)
            {
                if (stepType == "Given")
                {
                    Reporter.feature.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text).Fail(testError.Message);
                    //ScreenShotHelpers.CaptureScreen(DriverProvider.GetDriver, FeatureContext.Current.FeatureInfo.Title);
                }
                else if (stepType == "When")
                {
                    Reporter.feature.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text).Fail(testError.Message);
                    //ScreenShotHelpers.CaptureScreen(DriverProvider.GetDriver, FeatureContext.Current.FeatureInfo.Title);
                }
                else if (stepType == "Then")
                {
                    Reporter.feature.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text).Fail(testError.Message);
                    //ScreenShotHelpers.CaptureScreen(DriverProvider.GetDriver, FeatureContext.Current.FeatureInfo.Title);
                }
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            if (ScenarioContext.Current.TestError == null)
            {
                Reporter.scenario.Pass("Success");
            }
            else if (ScenarioContext.Current.TestError != null)
            {
                Reporter.scenario.Fail(ScenarioContext.Current.TestError.Message);
            }
        }

        [AfterFeature]
        public static void AfterFeature()
        {
            if (ScenarioContext.Current.TestError == null)
            {

            }
            else if (ScenarioContext.Current.TestError != null)
            {
                Reporter.feature.Fail("At least one scenario failed");
            }
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            DriverProvider.CloseDriver();
            Reporter.report.Flush();
        }
    }
}
