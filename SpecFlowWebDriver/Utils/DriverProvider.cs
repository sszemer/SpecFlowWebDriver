using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using SpecFlowWebDriver.Utis;
using System;
using System.IO;
using TechTalk.SpecFlow;

namespace SpecFlowWebDriver.Utils
{
    public static class DriverProvider
    {
        private static RemoteWebDriver driver;

        public static DriverType DriverType { get; set; }

        public static RemoteWebDriver GetDriver() => driver ?? InitDriver();

        public static void CloseDriver(ScenarioContext scenarioContext)
        {
            if (scenarioContext.Get<RemoteWebDriver>("driver") != null)
            {
                scenarioContext.Get<RemoteWebDriver>("driver").Quit();
                scenarioContext.Set<RemoteWebDriver>(null, "driver");
            }
        }
        //[MethodImpl(MethodImplOptions.Synchronized)]
        public static RemoteWebDriver InitDriver()
        {
            switch (DriverType)
            {
                case DriverType.Web:
                    driver = new RemoteWebDriver(EnvironmentHelper.TestEnvironment.HubURL, EnvironmentHelper.TestEnvironment.HubCapabilities, TimeSpan.FromSeconds(30));
                    driver.Manage().Window.Maximize();
                    break;
                case DriverType.Mobile:
                    driver = new RemoteWebDriver(EnvironmentHelper.TestEnvironment.HubURL, EnvironmentHelper.TestEnvironment.AppiumCapabilities, TimeSpan.FromSeconds(30));
                    break;
                case DriverType.Desktop:
                    break;
                case DriverType.None:
                    break;
                default:
                    break;
            }
            return driver;
        }

        public static string GetUrl(ScenarioContext scenarioContext)
        {
            string url;
            try
            {
                url = scenarioContext.Get<RemoteWebDriver>("driver")?.Url;
            }
            catch (Exception e)
            {
                url = $"Unable to get URL: {e.Message}";
            }
            return url;
        }

        public static string GetPageSource(ScenarioContext scenarioContext)
        {
            string returnvalue;
            string fileExtension = DriverType is DriverType.Mobile ? "xml" : "html";
            var pageSourceFileName = $"{RemoveCharactersUnsupportedByWindowsInFileNames(scenarioContext.StepContext.StepInfo.Text)}.{fileExtension}";
            var path = $"{AppDomain.CurrentDomain.BaseDirectory}..\\..\\..\\Report\\{pageSourceFileName}";
            try
            {
                var pageSource = scenarioContext.Get<RemoteWebDriver>("driver")?.PageSource;
                File.WriteAllText(path, pageSource);
                returnvalue = $"<a href=\"{path}\">{pageSourceFileName}</a>";
            }
            catch (Exception e)
            {
                returnvalue = $"Unable to get page source: {e.Message}";
            }
            return returnvalue;
        }

        public static string GetScreenshot(ScenarioContext scenarioContext)
        {
            string title = RemoveCharactersUnsupportedByWindowsInFileNames(scenarioContext.StepContext.StepInfo.Text);
            string Runname = $"{title}_{DateTime.Now:yyyy-MM-dd-HH_mm_ss}";
            string filename = $"{Runname}.jpg";
            string screenshotfilename = $"{AppDomain.CurrentDomain.BaseDirectory}..\\..\\..\\Report\\{filename}";
            try
            {
                Screenshot screenshot = ((ITakesScreenshot)scenarioContext.Get<RemoteWebDriver>("driver")).GetScreenshot();
                screenshot.SaveAsFile(screenshotfilename);
            }
            catch (Exception e)
            {
                screenshotfilename = $"Unable to get screenshot: {e.Message}";
            }
            return screenshotfilename;
        }

        private static string RemoveCharactersUnsupportedByWindowsInFileNames(string input)
        {
            return input.Replace(" ", "").Replace("\"", "").Replace("\\", "").Replace("/", "").Replace(":", "").Replace("*", "").Replace("?", "").Replace("<", "").Replace(">", "").Replace("'", "");
        }
    }
}
