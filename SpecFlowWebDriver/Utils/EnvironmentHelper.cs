using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
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
            if (testEnvironment == null) testEnvironment = new TestEnvironment();
            switch (environmentType)
            {
                case EnvironmentType.LOCAL:
                    testEnvironment.HubURL = new Uri("http://127.0.0.1:4444/wd/hub");
                    testEnvironment.HubCapabilities = SetHubCapabilities(environmentType);
                    testEnvironment.AppiumCapabilities = SetAppiumCapabilities(environmentType);
                    testEnvironment.AppiumOptions = SetAppiumOptions(environmentType);
                    break;
                case EnvironmentType.LOCAL_FROM_EXTERNAL_NETWORK:
                    testEnvironment.HubURL = new Uri($"http://{Environment.GetEnvironmentVariable("MY_EXTERNAL_IP")}:4444/wd/hub");
                    testEnvironment.HubCapabilities = SetHubCapabilities(environmentType);
                    testEnvironment.AppiumCapabilities = SetAppiumCapabilities(environmentType);
                    break;
                case EnvironmentType.LAMBDA_TEST:
                    testEnvironment.HubURL =
                        new Uri($"https://{Environment.GetEnvironmentVariable("LAMBDA_TEST_USERNAME")}:{Environment.GetEnvironmentVariable("LAMBDA_TEST_ACCESS_KEY")}@hub.lambdatest.com/wd/hub");
                    testEnvironment.HubCapabilities = SetHubCapabilities(environmentType);
                    testEnvironment.AppiumCapabilities = SetAppiumCapabilities(environmentType);
                    break;
            }
            return testEnvironment;
        }
        private static ICapabilities SetHubCapabilities(EnvironmentType environmentType)
        {
            Logger.Info("Environment Type: " + environmentType);
            ICapabilities result = null;
            switch (environmentType)
            {
                case EnvironmentType.LOCAL:
                    DriverOptions options = new ChromeOptions();
                    options.PlatformName = "windows";
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
            }
            return result;
        }
        private static ICapabilities SetAppiumCapabilities(EnvironmentType environmentType)
        {
            Logger.Info("Environment Type: " + environmentType);
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
            }
            return result;
        }
        private static AppiumOptions SetAppiumOptions(EnvironmentType environmentType)
        {
            Logger.Info("Environment Type: " + environmentType);
            AppiumOptions result = null;
            switch (environmentType)
            {
                case EnvironmentType.LOCAL:
                    AppiumOptions appiumOptionsLocal = new AppiumOptions();
                    appiumOptionsLocal.PlatformName = "Android";
                    appiumOptionsLocal.AddAdditionalCapability("appPackage", "com.android.chrome");
                    appiumOptionsLocal.AddAdditionalCapability("appActivity", "com.google.android.apps.chrome.Main");
                    //appiumOptionsLocal.AddAdditionalCapability("noReset", "true");
                    result = appiumOptionsLocal;
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
        public ICapabilities HubCapabilities { get; set; }
        public ICapabilities AppiumCapabilities { get; set; }
        public AppiumOptions AppiumOptions { get; set; }

    }

    public enum EnvironmentType
    {
        LOCAL,
        LOCAL_FROM_EXTERNAL_NETWORK,
        LAMBDA_TEST
    }
}
