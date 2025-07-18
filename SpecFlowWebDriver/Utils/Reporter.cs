using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using SpecFlowWebDriver.Utis;
using System.IO;

namespace SpecFlowWebDriver.Utils
{
    public static class Reporter
    {
        private static readonly string configFileName = $"{Path.Combine(Path.GetFullPath(Directory.GetCurrentDirectory()), "..", "..", "..", "Config", "extentReportConfig.xml")}";
        private static ExtentSparkReporter htmlReporter;
        private static ExtentReports report;
        private static ExtentTest feature;
        private static ExtentTest scenario;
        private static ExtentTest step;

        public static readonly string ReportDir = $"{Path.Combine(Path.GetFullPath(Directory.GetCurrentDirectory()), "..", "..", "..", "Report")}";
        public static ExtentTest Step { get => step; set => step = value; }
        public static ExtentTest Scenario { get => scenario; set => scenario = value; }
        public static ExtentTest Feature { get => feature; set => feature = value; }
        public static ExtentReports Report { get => report; set => report = value; }

        public static void SetupExtentReports()
        {
            InitHtmlReporter(new ExtentSparkReporter($"{Path.Combine(ReportDir, "index.html")}"));
            InitExtentReport(new ExtentReports());
            CleanReportDir(new DirectoryInfo(ReportDir));
        }

        private static void CleanReportDir(DirectoryInfo directoryInfo)
        {
            foreach (FileInfo file in directoryInfo.GetFiles()) file.Delete();
        }

        private static void InitHtmlReporter(ExtentSparkReporter extentSparkReporter)
        {
            htmlReporter = extentSparkReporter;
            htmlReporter.LoadConfig(configFileName);
        }

        private static void InitExtentReport(ExtentReports extentReports)
        {
            Report = extentReports;
            Report.AttachReporter(htmlReporter);
            Report.AddSystemInfo("OS", System.Environment.OSVersion.ToString());
            Report.AddSystemInfo("ENV", EnvironmentHelper.EnvironmentType.ToString());
            //if(DriverProvider.DriverType is DriverType.Web)report.AddSystemInfo("Browser", $"{DriverProvider.Driver?.Capabilities["browserName"]} {DriverProvider.Driver?.Capabilities["browserVersion"]}");
        }
    }
}
