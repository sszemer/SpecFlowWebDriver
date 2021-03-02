using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;

namespace SpecFlowWebDriver.Pages
{
    public abstract class CommonAndroidPage : CommonAbstractPage
    {
        protected AppiumDriver<AppiumWebElement> driver;
        protected WebDriverWait wait;

        public CommonAndroidPage(AppiumDriver<AppiumWebElement> driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        }

        protected void WaitAndClick(By elementBy) => wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(elementBy)).Click();
    }
}
