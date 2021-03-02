using OpenQA.Selenium.Remote;

namespace SpecFlowWebDriver.Pages
{
    public abstract class CommonWebPage : CommonAbstractPage
    {
        protected RemoteWebDriver driver;

        public CommonWebPage(RemoteWebDriver driver)
        {
            this.driver = driver;
        }
    }
}
