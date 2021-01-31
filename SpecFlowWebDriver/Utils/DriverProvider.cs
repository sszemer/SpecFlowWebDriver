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
        public static void CloseDriver(ScenarioContext scenarioContext)
        {
            if (scenarioContext.TryGetValue<RemoteWebDriver>("driver", out RemoteWebDriver driver))
            {
                driver.Quit();
            }
        }
        public static void InitDriver(ScenarioContext scenarioContext)
        {
            switch (scenarioContext.Get<DriverType>())
            {
                case DriverType.Web:
                    scenarioContext.Set<RemoteWebDriver>(new RemoteWebDriver(EnvironmentHelper.TestEnvironment.HubURL, EnvironmentHelper.TestEnvironment.HubCapabilities, TimeSpan.FromSeconds(30)), "driver");
                    scenarioContext.Get<RemoteWebDriver>("driver").Manage().Window.Maximize();
                    break;
                case DriverType.Mobile:
                    scenarioContext.Set<RemoteWebDriver>(new RemoteWebDriver(EnvironmentHelper.TestEnvironment.HubURL, EnvironmentHelper.TestEnvironment.AppiumCapabilities, TimeSpan.FromSeconds(30)), "driver");
                    break;
                case DriverType.Desktop:
                    //todo implement winium?
                    break;
                case DriverType.None:
                    break;
                default:
                    break;
            }
        }

        public static string GetUrl(ScenarioContext scenarioContext)
        {
            string url = string.Empty;
            if (scenarioContext.TryGetValue<RemoteWebDriver>("driver", out RemoteWebDriver driver))
            {
                try
                {
                    url = driver?.Url;
                    return url;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unable to get URL: {e.Message}");
                }
            }
            return null;
        }

        public static string GetPageSource(ScenarioContext scenarioContext)
        {
            string fileExtension = scenarioContext.Get<DriverType>() is DriverType.Mobile ? "xml" : "html";
            var pageSourceFileName = $"{RemoveCharactersUnsupportedByWindowsInFileNames(scenarioContext.StepContext.StepInfo.Text)}.{fileExtension}";
            var path = $"{Path.Combine(Reporter.ReportDir, pageSourceFileName)}";
            if (scenarioContext.TryGetValue<RemoteWebDriver>("driver", out RemoteWebDriver driver))
            {
                try
                {
                    var pageSource = driver?.PageSource;
                    File.WriteAllText(path, pageSource);
                    return pageSourceFileName;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unable to get page source: {e.Message}");
                }
            }
            return null;
        }

        public static string GetScreenshot(ScenarioContext scenarioContext)
        {
            string title = RemoveCharactersUnsupportedByWindowsInFileNames(scenarioContext.StepContext.StepInfo.Text);
            string Runname = $"{title}_{DateTime.Now:yyyy-MM-dd-HH_mm_ss}";
            string filename = $"{Runname}.jpg";
            string path = $"{Path.Combine(Reporter.ReportDir, filename)}";
            if (scenarioContext.TryGetValue<RemoteWebDriver>("driver", out RemoteWebDriver driver))
            {
                try
                {
                    Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                    screenshot.SaveAsFile(path);
                    return filename;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unable to get screenshot: {e.Message}");
                }
            }
            return null;
        }

        private static string RemoveCharactersUnsupportedByWindowsInFileNames(string input)
        {
            return input.Replace(" ", "").Replace("\"", "").Replace("\\", "").Replace("/", "").Replace(":", "").Replace("*", "").Replace("?", "").Replace("<", "").Replace(">", "").Replace("'", "");
        }
    }
}
