using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace SpecFlowWebDriver.Pages
{
    public class ChromePage : CommonAndroidPage
    {
        public ChromePage(AppiumDriver<AppiumWebElement> driver) : base(driver) { }
        public By SearchInput => By.Id("com.android.chrome:id/search_box_text");
        public IWebElement AcceptTerms => driver.FindElement(By.Id("com.android.chrome:id/terms_accept"));
        public By NoThanks => By.Id("com.android.chrome:id/negative_button");
        public IWebElement UrlBar => driver.FindElement(By.Id("com.android.chrome:id/url_bar"));
        public IWebElement KeyboardReturn => driver.FindElement(By.Id("com.android.chrome:id/url_bar"));
        public IWebElement GoogleInput => driver.FindElement(By.ClassName("android.widget.EditText"));
        public IWebElement FirstSearchHint => driver.FindElement(By.Id("com.android.chrome:id/line_1"));
        public void GoogleThing(string thing)
        {
            AcceptTerms.Click();
            WaitAndClick(NoThanks);
            WaitAndClick(SearchInput);
            UrlBar.Clear();
            UrlBar.SendKeys(thing); 
            ((AndroidDriver<AppiumWebElement>)driver).PressKeyCode(AndroidKeyCode.Enter);
            //for RemoteWebDriver
            //driver.ExecuteScript("mobile:scroll", new Dictionary<string, string> { { "action", "search" } });
            //FirstSearchHint.Click();
        }
    }
}
