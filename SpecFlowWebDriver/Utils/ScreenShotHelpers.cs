using OpenQA.Selenium;
using System;
using TechTalk.SpecFlow;

namespace SpecFlowWebDriver.Utils
{
    static class ScreenShotHelpers
    {
        public static string CaptureScreen()
        {
            Screenshot screenshot = ((ITakesScreenshot)DriverProvider.GetDriver()).GetScreenshot();
            string title = ScenarioStepContext.Current.StepInfo.Text.Replace(" ", "");
            string Runname = $"{title}_{DateTime.Now:yyyy-MM-dd-HH_mm_ss}";
            string screenshotfilename = $"{Runname}.jpg";
            screenshot.SaveAsFile(screenshotfilename);
            return screenshotfilename;
        }
    }
}
