using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System.Threading;

namespace SpecFlowWebDriver.Pages
{
    public class ChromePage : CommonPage
    {
        public ChromePage(RemoteWebDriver driver) : base(driver) { }
        public IWebElement SearchInput => driver.FindElement(By.XPath("//android.widget.EditText[@resource-id='com.android.chrome:id/search_box_text']"));
        public IWebElement AcceptTerms => driver.FindElement(By.XPath("//android.widget.Button[@resource-id='com.android.chrome:id/terms_accept']"));
        public IWebElement NoThanks => driver.FindElement(By.XPath("//android.widget.Button[@resource-id='com.android.chrome:id/negative_button']"));
        public IWebElement UrlBar => driver.FindElement(By.XPath("//android.widget.EditText[@resource-id='com.android.chrome:id/url_bar']"));
        public IWebElement GoogleInput => driver.FindElement(By.ClassName("android.widget.EditText"));
        public IWebElement FirstSearchHint => driver.FindElement(By.XPath("//android.widget.TextView[@resource-id='com.android.chrome:id/line_1']"));
        public void GoogleThing(string thing)
        {
            AcceptTerms.Click();
            NoThanks.Click();
            Thread.Sleep(5000);
            SearchInput.Click();
            UrlBar.Clear();
            UrlBar.SendKeys(thing);
            FirstSearchHint.Click();
        }
    }
}
