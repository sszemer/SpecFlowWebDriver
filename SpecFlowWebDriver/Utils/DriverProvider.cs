using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace SpecFlowWebDriver.Utils
{
    static class DriverProvider
    {
        private static RemoteWebDriver driver;
        private static DriverOptions options;

        public static RemoteWebDriver GetDriver()
        {
            if (driver == null)
            {
                options = new ChromeOptions();
                driver = new ChromeDriver((ChromeOptions)options);
                driver.Manage().Window.Maximize();
            }
            return driver;
        }

        public static void CloseDriver()
        {
            driver.Close();
            driver.Dispose();
            driver.Quit();
            driver = null;
        }
    }
}
