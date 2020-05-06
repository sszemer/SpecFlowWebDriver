using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using NUnit.Framework;
using System;
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
            switch(ScenarioContext.Current.StepContext.StepInfo.StepDefinitionType)
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
        public void AfterStep()
        {
            var pageSource = DriverProvider.GetDriver().PageSource;
            var path = "c:\\temp\\a.html";
            File.WriteAllText(path, pageSource);
            if (ScenarioContext.Current.TestError != null)
            {
                Reporter.step.Fail(ScenarioContext.Current.TestError.Message + "<br/><a href=\"" + path + "\">Page source</a>");
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
