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

        public static RemoteWebDriver Driver => driver ?? InitDriver();

        public static void CloseDriver(ScenarioContext scenarioContext)
        {
            scenarioContext.TryGetValue<RemoteWebDriver>("driver", out driver);
            if (driver != null)
            {
                driver.Quit();
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
            string url = string.Empty;
            if (scenarioContext.TryGetValue<RemoteWebDriver>("driver", out driver))
            {
                try
                {
                    url = driver?.Url;
                }
                catch (Exception e)
                {
                    url = $"Unable to get URL: {e.Message}";
                }
            }
            return url;
        }

        public static string GetPageSource(ScenarioContext scenarioContext)
        {
            string returnValue = string.Empty;
            string fileExtension = DriverType is DriverType.Mobile ? "xml" : "html";
            var pageSourceFileName = $"{RemoveCharactersUnsupportedByWindowsInFileNames(scenarioContext.StepContext.StepInfo.Text)}.{fileExtension}";
            var path = $"{Path.Combine(Reporter.ReportDir, pageSourceFileName)}";
            if (scenarioContext.TryGetValue<RemoteWebDriver>("driver", out driver))
            {
                try
                {
                    var pageSource = driver?.PageSource;
                    File.WriteAllText(path, pageSource);
                    returnValue = $"<a href=\"{pageSourceFileName}\">{pageSourceFileName}</a>";
                }
                catch (Exception e)
                {
                    returnValue = $"Unable to get page source: {e.Message}";
                }
            }
            return returnValue;
        }

        public static string GetScreenshot(ScenarioContext scenarioContext)
        {
            string title = RemoveCharactersUnsupportedByWindowsInFileNames(scenarioContext.StepContext.StepInfo.Text);
            string Runname = $"{title}_{DateTime.Now:yyyy-MM-dd-HH_mm_ss}";
            string filename = $"{Runname}.jpg";
            string screenshotfilename = $"{Path.Combine(Reporter.ReportDir, filename)}";
            if (scenarioContext.TryGetValue<RemoteWebDriver>("driver", out driver))
            {
                try
                {
                    Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                    screenshot.SaveAsFile(screenshotfilename);
                }
                catch (Exception e)
                {
                    screenshotfilename = $"Unable to get screenshot: {e.Message}";
                }
            }
            return filename;
        }

        private static string RemoveCharactersUnsupportedByWindowsInFileNames(string input)
        {
            return input.Replace(" ", "").Replace("\"", "").Replace("\\", "").Replace("/", "").Replace(":", "").Replace("*", "").Replace("?", "").Replace("<", "").Replace(">", "").Replace("'", "");
        }
    }
}
