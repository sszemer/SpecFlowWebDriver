using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;

namespace SpecFlowWebDriver.Utils
{
    static class DriverProvider
    {
        private static RemoteWebDriver driver;
        private static DriverOptions options;
        private static string hubURL = "http://localhost:4444/wd/hub";
        private static string appiumUrl = "http://localhost:4723/wd/hub";

        public static RemoteWebDriver GetDriver()
        {
            if (driver == null)
            {
                InitWebDriver();
            }
            return driver;
        }

        public static void CloseDriver()
        {
            driver.Quit();
            driver = null;
        }

        public static void InitWebDriver()
        {
            var driverType = "chrome";
            switch (driverType)
            {
                case "chrome":
                    options = new ChromeOptions();
                    options.PlatformName = "windows";
                    driver = new RemoteWebDriver(new Uri(hubURL), options.ToCapabilities(), TimeSpan.FromSeconds(600));
                    driver.Manage().Window.Maximize();
                    break;
                case "appium":
                    options = new AppiumOptions();
                    options.PlatformName = "Android";
                    options.AddAdditionalCapability("appPackage", "com.android.chrome");
                    options.AddAdditionalCapability("appActivity", "com.google.android.apps.chrome.Main");
                    driver = new AndroidDriver<AppiumWebElement>(new Uri(appiumUrl), options, TimeSpan.FromSeconds(600));
                    break;
                default:
                    break;
            }
        }
    }
}
