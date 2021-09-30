using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;

namespace SpecFlowWebDriver.Utis
{
    public static class EnvironmentHelper
    {
        private static TestEnvironment testEnvironment;
        private static EnvironmentType environmentType;
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public static TestEnvironment TestEnvironment { get => SetEnvironment(environmentType); }
        public static EnvironmentType EnvironmentType { get => environmentType; set => environmentType = value; }
        private static TestEnvironment SetEnvironment(EnvironmentType environmentType)
        {
            Logger.Info("Environment Type: " + environmentType);
            testEnvironment ??= new TestEnvironment();
            testEnvironment.WebCapabilities = SetWebCapabilities(environmentType);
            testEnvironment.AppiumCapabilities = SetAppiumCapabilities(environmentType);
            testEnvironment.AppiumOptions = SetAppiumOptions(environmentType);
            testEnvironment.HubURL = SetHubUrl(environmentType);
            return testEnvironment;
        }
        private static Uri SetHubUrl(EnvironmentType environmentType)
        {
            Logger.Info("Setting HubUrl for Environment Type: " + environmentType);
            Uri result = null;
            switch (environmentType)
            {
                case EnvironmentType.LOCAL:
                    result = new Uri("http://127.0.0.1:4444/wd/hub");
                    break;
                case EnvironmentType.LOCAL_FROM_EXTERNAL_NETWORK:
                    result = new Uri($"http://{Environment.GetEnvironmentVariable("MY_EXTERNAL_IP")}:4444/wd/hub");
                    break;
                case EnvironmentType.LAMBDA_TEST:
                    result =
                        new Uri($"https://{Environment.GetEnvironmentVariable("LAMBDA_TEST_USERNAME")}:{Environment.GetEnvironmentVariable("LAMBDA_TEST_ACCESS_KEY")}@hub.lambdatest.com/wd/hub");
                    break;
                case EnvironmentType.BROWSERSTACK:
                    testEnvironment.HubURL =
                        new Uri("https://hub-cloud.browserstack.com/wd/hub/");
                    break;
            }
            return result;
        }
        private static ICapabilities SetWebCapabilities(EnvironmentType environmentType)
        {
            Logger.Info("Setting WebCapabilities for Environment Type: " + environmentType);
            ICapabilities result = null;
            switch (environmentType)
            {
                case EnvironmentType.LOCAL:
                    DriverOptions options = new ChromeOptions();
                    //options.PlatformName = "windows";
                    result = options.ToCapabilities();
                    break;
                case EnvironmentType.LOCAL_FROM_EXTERNAL_NETWORK:
                    DriverOptions localOptionsFromExternal = new ChromeOptions();
                    localOptionsFromExternal.PlatformName = "windows";
                    result = localOptionsFromExternal.ToCapabilities();
                    break;
                case EnvironmentType.LAMBDA_TEST:
                    DesiredCapabilities dCapsLambdaTest = new DesiredCapabilities();
                    dCapsLambdaTest.SetCapability("user", Environment.GetEnvironmentVariable("LAMBDA_TEST_USERNAME"));
                    dCapsLambdaTest.SetCapability("accessKey", Environment.GetEnvironmentVariable("LAMBDA_TEST_ACCESS_KEY"));
                    dCapsLambdaTest.SetCapability("build", DateTime.Now.ToString());
                    dCapsLambdaTest.SetCapability("name", "Multiplatform Selenium Grid");
                    dCapsLambdaTest.SetCapability("platform", "Windows 10");
                    dCapsLambdaTest.SetCapability("browserName", "Chrome");
                    dCapsLambdaTest.SetCapability("version", "88.0");
                    dCapsLambdaTest.SetCapability("console", true);
                    dCapsLambdaTest.SetCapability("network", true);
                    dCapsLambdaTest.SetCapability("video", true);
                    result = dCapsLambdaTest;
                    break;
                case EnvironmentType.BROWSERSTACK:
                    DesiredCapabilities dCapsBrowserStack = new DesiredCapabilities();
                    dCapsBrowserStack.SetCapability("browserstack.user", Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME"));
                    dCapsBrowserStack.SetCapability("browserstack.key", Environment.GetEnvironmentVariable("BROWSERSTACK_AUTOMATE_KEY"));
                    dCapsBrowserStack.SetCapability("name", "sebastianszemer1's First Test");
                    dCapsBrowserStack.SetCapability("os", "Windows");
                    dCapsBrowserStack.SetCapability("os_version", "10");
                    dCapsBrowserStack.SetCapability("browser", "Chrome");
                    dCapsBrowserStack.SetCapability("browser_version", "88");
                    result = dCapsBrowserStack;
                    break;
            }
            return result;
        }
        private static ICapabilities SetAppiumCapabilities(EnvironmentType environmentType)
        {
            Logger.Info("Setting Appium Capabilities for Environment Type: " + environmentType);
            ICapabilities result = null;
            switch (environmentType)
            {
                case EnvironmentType.LOCAL:
                    DesiredCapabilities dCapsLocal = new DesiredCapabilities();
                    dCapsLocal.SetCapability("platformName", "Android");
                    dCapsLocal.SetCapability("appPackage", "com.android.chrome");
                    dCapsLocal.SetCapability("appActivity", "com.google.android.apps.chrome.Main");
                    result = dCapsLocal;
                    break;
                case EnvironmentType.LOCAL_FROM_EXTERNAL_NETWORK:
                    DesiredCapabilities dCapsLocalFromExternal = new DesiredCapabilities();
                    dCapsLocalFromExternal.SetCapability("platformName", "Android");
                    dCapsLocalFromExternal.SetCapability("appPackage", "com.android.chrome");
                    dCapsLocalFromExternal.SetCapability("appActivity", "com.google.android.apps.chrome.Main");
                    result = dCapsLocalFromExternal;
                    break;
                case EnvironmentType.LAMBDA_TEST:
                    DesiredCapabilities dCapsLambdaTest = new DesiredCapabilities();
                    dCapsLambdaTest.SetCapability("user", Environment.GetEnvironmentVariable("LAMBDA_TEST_USERNAME"));
                    dCapsLambdaTest.SetCapability("accessKey", Environment.GetEnvironmentVariable("LAMBDA_TEST_ACCESS_KEY"));
                    dCapsLambdaTest.SetCapability("build", DateTime.Now.ToString());
                    dCapsLambdaTest.SetCapability("name", "Multiplatform Selenium Grid");
                    dCapsLambdaTest.SetCapability("platformName", "Android");
                    dCapsLambdaTest.SetCapability("deviceName", "Galaxy S9");
                    dCapsLambdaTest.SetCapability("platformVersion", "10");
                    dCapsLambdaTest.SetCapability("console", true);
                    dCapsLambdaTest.SetCapability("network", true);
                    dCapsLambdaTest.SetCapability("visual", true);
                    result = dCapsLambdaTest;
                    break;
                case EnvironmentType.BROWSERSTACK:
                    DesiredCapabilities dCapsBrowserStack = new DesiredCapabilities();
                    dCapsBrowserStack.SetCapability("browserstack.user", Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME"));
                    dCapsBrowserStack.SetCapability("browserstack.key", Environment.GetEnvironmentVariable("BROWSERSTACK_AUTOMATE_KEY"));
                    dCapsBrowserStack.SetCapability("browserName", "Chrome");
                    dCapsBrowserStack.SetCapability("platformName", "android");
                    dCapsBrowserStack.SetCapability("name", "sebastianszemer1's First Test");
                    dCapsBrowserStack.SetCapability("device", "Samsung Galaxy S20");
                    dCapsBrowserStack.SetCapability("realMobile", true);
                    dCapsBrowserStack.SetCapability("os_version", "11.0");
                    result = dCapsBrowserStack;
                    break;
            }
            return result;
        }
        private static DriverOptions SetAppiumOptions(EnvironmentType environmentType)
        {
            Logger.Info("Setting AppiumOptions for Environment Type: " + environmentType);
            DriverOptions result = null;
            switch (environmentType)
            {
                case EnvironmentType.LOCAL:
                    DriverOptions appiumOptionsLocal = new AppiumOptions();
                    appiumOptionsLocal.PlatformName = "Android";
                    appiumOptionsLocal.AddAdditionalCapability("appPackage", "com.android.chrome");
                    appiumOptionsLocal.AddAdditionalCapability("appActivity", "com.google.android.apps.chrome.Main");
                    //appiumOptionsLocal.AddAdditionalCapability("noReset", "true");
                    result = appiumOptionsLocal;
                    break;
                case EnvironmentType.LOCAL_FROM_EXTERNAL_NETWORK:
                    DriverOptions appiumOptionsLocalFEN = new AppiumOptions();
                    appiumOptionsLocalFEN.PlatformName = "Android";
                    appiumOptionsLocalFEN.AddAdditionalCapability("appPackage", "com.android.chrome");
                    appiumOptionsLocalFEN.AddAdditionalCapability("appActivity", "com.google.android.apps.chrome.Main");
                    //appiumOptionsLocal.AddAdditionalCapability("noReset", "true");
                    result = appiumOptionsLocalFEN;
                    break;
                case EnvironmentType.BROWSERSTACK:
                    DriverOptions appiumOptionsBrowserStack = new AppiumOptions();
                    appiumOptionsBrowserStack.AddAdditionalCapability("browserstack.user", Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME"));
                    appiumOptionsBrowserStack.AddAdditionalCapability("browserstack.key", Environment.GetEnvironmentVariable("BROWSERSTACK_AUTOMATE_KEY"));
                    appiumOptionsBrowserStack.AddAdditionalCapability("browserName", "android");
                    appiumOptionsBrowserStack.AddAdditionalCapability("name", "sebastianszemer1's First Test");
                    appiumOptionsBrowserStack.AddAdditionalCapability("device", "Samsung Galaxy S20");
                    appiumOptionsBrowserStack.AddAdditionalCapability("realMobile", true);
                    appiumOptionsBrowserStack.AddAdditionalCapability("os_version", "11.0");
                    appiumOptionsBrowserStack.AddAdditionalCapability("app", "bs://c700ce60cf13ae8ed97705a55b8e022f13c5827c");
                    result = appiumOptionsBrowserStack;
                    break;
                case EnvironmentType.LAMBDA_TEST:
                    DriverOptions optionsLambdaTest = new AppiumOptions();
                    optionsLambdaTest.AddAdditionalCapability("user", Environment.GetEnvironmentVariable("LAMBDA_TEST_USERNAME"));
                    optionsLambdaTest.AddAdditionalCapability("accessKey", Environment.GetEnvironmentVariable("LAMBDA_TEST_ACCESS_KEY"));
                    optionsLambdaTest.AddAdditionalCapability("build", DateTime.Now.ToString());
                    optionsLambdaTest.AddAdditionalCapability("name", "Multiplatform Selenium Grid");
                    optionsLambdaTest.AddAdditionalCapability("platformName", "Android");
                    optionsLambdaTest.AddAdditionalCapability("deviceName", "Galaxy S9");
                    optionsLambdaTest.AddAdditionalCapability("platformVersion", "10");
                    optionsLambdaTest.AddAdditionalCapability("console", true);
                    optionsLambdaTest.AddAdditionalCapability("network", true);
                    optionsLambdaTest.AddAdditionalCapability("visual", true);
                    result = optionsLambdaTest;
                    break;
                default:
                    throw new Exception($"{environmentType} does not support AppiumOptions");
            }
            return result;
        }
    }

    public class TestEnvironment
    {
        public Uri HubURL { get; set; }
        public ICapabilities WebCapabilities { get; set; }
        public ICapabilities AppiumCapabilities { get; set; }
        public DriverOptions AppiumOptions { get; set; }

    }

    public enum EnvironmentType
    {
        LOCAL,
        LOCAL_FROM_EXTERNAL_NETWORK,
        LAMBDA_TEST,
        BROWSERSTACK
    }
}
