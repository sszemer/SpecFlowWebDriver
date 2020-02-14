using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;

namespace SpecFlowWebDriver.Utils
{
    static class Reporter
    {
        public static AventStack.ExtentReports.ExtentReports report;
        public static ExtentHtmlReporter htmlReporter;
        public static ExtentTest feature;
        public static ExtentTest scenario;

        public static void SetupExtentHtmlReporter()
        {
            htmlReporter = new ExtentHtmlReporter("testReport.html");
            htmlReporter.Config.Theme = Theme.Dark;
            htmlReporter.Config.DocumentTitle = "Test Report";
            htmlReporter.Config.ReportName = "SpecFlow Tests";
            report = new AventStack.ExtentReports.ExtentReports();
            report.AttachReporter(htmlReporter);
            report.AddSystemInfo("OS", "Windows 10");
        }
    }
}
