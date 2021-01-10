using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using TechTalk.SpecFlow;

namespace SpecFlowWebDriver.Utils
{
    public static class DriverProvider
    {
        private static readonly string testHost = "127.0.0.1";
        private static RemoteWebDriver driver;
        private static DriverOptions options;
        private static readonly string hubURL = $"http://{testHost}:4444/wd/hub";
        private static readonly string appiumUrl = $"http://{testHost}:4723/wd/hub";

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
                    options = new ChromeOptions();
                    options.PlatformName = "windows";
                    driver = new RemoteWebDriver(new Uri(hubURL), options.ToCapabilities(), TimeSpan.FromSeconds(600));
                    driver.Manage().Window.Maximize();
                    break;
                case DriverType.Mobile:
                    options = new AppiumOptions();
                    options.PlatformName = "Android";
                    options.AddAdditionalCapability("appPackage", "com.android.chrome");
                    options.AddAdditionalCapability("appActivity", "com.google.android.apps.chrome.Main");
                    driver = new AndroidDriver<AppiumWebElement>(new Uri(appiumUrl), options, TimeSpan.FromSeconds(600));
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
            try
            {
                var pageSource = scenarioContext.Get<RemoteWebDriver>("driver")?.PageSource;
                var pageSourceFileName = $"{scenarioContext.StepContext.StepInfo.Text}.{fileExtension}";
                var path = $"{AppDomain.CurrentDomain.BaseDirectory}..\\..\\..\\Report\\{pageSourceFileName}";
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
            string screenshotfilename;
            try
            {
                Screenshot screenshot = ((ITakesScreenshot)scenarioContext.Get<RemoteWebDriver>("driver")).GetScreenshot();
                string title = scenarioContext.StepContext.StepInfo.Text.Replace(" ", "");
                string Runname = $"{title}_{DateTime.Now:yyyy-MM-dd-HH_mm_ss}";
                screenshotfilename = $"{AppDomain.CurrentDomain.BaseDirectory}..\\..\\..\\Report\\{Runname}.jpg";
                screenshot.SaveAsFile(screenshotfilename);
            }
            catch (Exception e)
            {
                screenshotfilename = $"Unable to get screenshot: {e.Message}";
            }
            return screenshotfilename;
        }
    }
}
