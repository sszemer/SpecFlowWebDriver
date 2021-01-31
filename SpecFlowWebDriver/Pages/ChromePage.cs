﻿using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;

namespace SpecFlowWebDriver.Pages
{
    public class ChromePage : CommonPage
    {
        public ChromePage(RemoteWebDriver driver) : base(driver) { }
        public IWebElement SearchInput => driver.FindElement(By.Id("com.android.chrome:id/search_box_text"));
        public IWebElement AcceptTerms => driver.FindElement(By.Id("com.android.chrome:id/terms_accept"));
        public IWebElement NoThanks => driver.FindElement(By.Id("com.android.chrome:id/negative_button"));
        public IWebElement UrlBar => driver.FindElement(By.Id("com.android.chrome:id/url_bar"));
        public IWebElement KeyboardReturn => driver.FindElement(By.Id("com.android.chrome:id/url_bar"));
        public IWebElement GoogleInput => driver.FindElement(By.ClassName("android.widget.EditText"));
        public void GoogleThing(string thing)
        {
            AcceptTerms.Click();
            NoThanks.Click();
            var wait = new WebDriverWait(driver,TimeSpan.FromSeconds(30));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("com.android.chrome:id/search_box_text")));
            SearchInput.Click();
            UrlBar.Clear();
            UrlBar.SendKeys(thing);
            ((AndroidDriver<AppiumWebElement>)driver).PressKeyCode(AndroidKeyCode.Enter);
        }
    }
}