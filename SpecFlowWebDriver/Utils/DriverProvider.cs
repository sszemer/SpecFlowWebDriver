using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;

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

        public static void CloseDriver()
        {
            if (driver != null)
            {
                driver.Quit();
                driver = null;
            }
        }

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
    }
}
