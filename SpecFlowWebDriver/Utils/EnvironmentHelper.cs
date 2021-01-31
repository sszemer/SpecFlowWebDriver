using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;

namespace SpecFlowWebDriver.Utis
{
    public static class EnvironmentHelper
    {
        private static TestEnvironment testEnvironment;
        private static EnvironmentType environmentType;

        public static TestEnvironment TestEnvironment { get => SetEnvironment(environmentType); }
        public static EnvironmentType EnvironmentType { get => environmentType; set => environmentType = value; }
        private static TestEnvironment SetEnvironment(EnvironmentType environmentType)
        {
            if (testEnvironment == null) testEnvironment = new TestEnvironment();
            switch (environmentType)
            {
                case EnvironmentType.LOCAL:
                    testEnvironment.HubURL = new Uri("http://127.0.0.1:4444/wd/hub");
                    testEnvironment.HubCapabilities = SetHubCapabilities(environmentType);
                    testEnvironment.AppiumCapabilities = SetAppiumCapabilities(environmentType);
                    break;
                case EnvironmentType.LAMBDA_TEST:
                    testEnvironment.HubURL =
                        new Uri($"https://{Environment.GetEnvironmentVariable("LAMBDA_TEST_USERNAME")}:{Environment.GetEnvironmentVariable("LAMBDA_TEST_ACCESS_KEY")}@hub.lambdatest.com/wd/hub");
                    testEnvironment.HubCapabilities = SetHubCapabilities(environmentType);
                    break;
            }
            return testEnvironment;
        }
        private static ICapabilities SetHubCapabilities(EnvironmentType environmentType)
        {
            ICapabilities result = null;
            switch (environmentType)
            {
                case EnvironmentType.LOCAL:
                    DriverOptions options = new ChromeOptions();
                    options.PlatformName = "windows";
                    result = options.ToCapabilities();
                    break;
                case EnvironmentType.LAMBDA_TEST:
                    DesiredCapabilities t = new DesiredCapabilities();
                    t.SetCapability("user", Environment.GetEnvironmentVariable("LAMBDA_TEST_USERNAME"));
                    t.SetCapability("accessKey", Environment.GetEnvironmentVariable("LAMBDA_TEST_ACCESS_KEY"));
                    t.SetCapability("build", DateTime.Now.ToString());
                    t.SetCapability("name", "Multiplatform Selenium Grid");
                    t.SetCapability("platform", "Windows 10");
                    t.SetCapability("browserName", "Chrome");
                    t.SetCapability("version", "88.0");
                    t.SetCapability("console", true);
                    t.SetCapability("network", true);
                    t.SetCapability("video", true);
                    result = t;
                    break;
            }
            return result;
        }
        private static ICapabilities SetAppiumCapabilities(EnvironmentType environmentType)
        {
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
                case EnvironmentType.LAMBDA_TEST:
                    DesiredCapabilities dCapsLambdaTest = new DesiredCapabilities();
                    dCapsLambdaTest.SetCapability("user", Environment.GetEnvironmentVariable("LAMBDA_TEST_USERNAME"));
                    dCapsLambdaTest.SetCapability("accessKey", Environment.GetEnvironmentVariable("LAMBDA_TEST_ACCESS_KEY"));
                    dCapsLambdaTest.SetCapability("build", DateTime.Now.ToString());
                    dCapsLambdaTest.SetCapability("name", "Multiplatform Selenium Grid");
                    dCapsLambdaTest.SetCapability("platformName", "Android");
                    dCapsLambdaTest.SetCapability("deviceName", "Galaxy S9");
                    dCapsLambdaTest.SetCapability("platformVersion", "10");
                    dCapsLambdaTest.SetCapability("appiumVersion", "1.8.0");
                    dCapsLambdaTest.SetCapability("console", true);
                    dCapsLambdaTest.SetCapability("network", true);
                    dCapsLambdaTest.SetCapability("visual", true);
                    result = dCapsLambdaTest;
                    break;
            }
            return result;
        }
    }

    public class TestEnvironment
    {
        public Uri HubURL { get; set; }
        public ICapabilities HubCapabilities { get; set; }
        public ICapabilities AppiumCapabilities { get; set; }

    }

    public enum EnvironmentType
    {
        LOCAL,
        LAMBDA_TEST
    }
}
