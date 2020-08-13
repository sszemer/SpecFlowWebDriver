using System;
using System.IO;
using TechTalk.SpecFlow;

namespace SpecFlowWebDriver.Utils
{
    static class PageSourceHelper
    {
        public static string GetPageSource()
        {
            var pageSource = DriverProvider.GetDriver().PageSource;
            var pageSourceFileName = $"{ScenarioStepContext.Current.StepInfo.Text}.html";
            var path = $"{AppDomain.CurrentDomain.BaseDirectory}..\\..\\..\\Report\\{pageSourceFileName}";
            File.WriteAllText(path, pageSource);
            return $"<a href=\"{path}\">{pageSourceFileName}</a>";
        }
    }
}
