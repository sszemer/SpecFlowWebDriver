using OpenQA.Selenium.Remote;

namespace SpecFlowWebDriver.Pages
{
    public abstract class CommonPage
    {
        public RemoteWebDriver driver;

        public CommonPage(RemoteWebDriver driver)
        {
            this.driver = driver;
        }
    }
}
