using OpenQA.Selenium;
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

        public static RemoteWebDriver GetDriver()
        {
            if (driver == null)
            {
                options = new ChromeOptions();
                options.PlatformName = "windows";
                driver = new RemoteWebDriver(new Uri(hubURL), options.ToCapabilities(), TimeSpan.FromSeconds(600));
                driver.Manage().Window.Maximize();
            }
            return driver;
        }

        public static void CloseDriver()
        {
            driver.Quit();
            driver = null;
        }
    }
}
