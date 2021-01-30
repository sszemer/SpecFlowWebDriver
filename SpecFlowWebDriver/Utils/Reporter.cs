using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using SpecFlowWebDriver.Utis;
using System.IO;

namespace SpecFlowWebDriver.Utils
{
    public static class Reporter
    {
        private const string reportTitle = "Test Report";
        private const string reportName = "SpecFlow Tests";
        private const string klovURL = "http://127.0.0.1";
        private const string mongoURL = "127.0.0.1";
        private const int mongoPort = 27017;
        private static readonly string configFileName = $"{Path.Combine(Path.GetFullPath(Directory.GetCurrentDirectory()), "..", "..", "..", "Config", "extentReportConfig.xml")}";
        private static ExtentHtmlReporter htmlReporter;
        private static ExtentKlovReporter klov;
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
            InitHtmlReporter(new ExtentHtmlReporter($"{Path.Combine(ReportDir, "index.html")}"));
            if (EnvironmentHelper.EnvironmentType is EnvironmentType.LOCAL) InitKlovReporter(new ExtentKlovReporter());
            InitExtentReport(new ExtentReports());
            CleanReportDir(new DirectoryInfo(ReportDir));
        }

        private static void CleanReportDir(DirectoryInfo directoryInfo)
        {
            foreach (FileInfo file in directoryInfo.GetFiles()) file.Delete();
        }

        private static void InitHtmlReporter(ExtentHtmlReporter extentHtmlReporter)
        {
            htmlReporter = extentHtmlReporter;
            htmlReporter.LoadConfig(configFileName);
        }
        private static void InitKlovReporter(ExtentKlovReporter extentKlovReporter)
        {
            klov = extentKlovReporter;
            klov.InitMongoDbConnection(mongoURL, mongoPort);
            klov.InitKlovServerConnection(klovURL);
            klov.ProjectName = reportTitle;
            klov.ReportName = reportName;
        }

        private static void InitExtentReport(ExtentReports extentReports)
        {
            Report = extentReports;
            Report.AttachReporter(htmlReporter);
            if (EnvironmentHelper.EnvironmentType is EnvironmentType.LOCAL) Report.AttachReporter(klov);
            Report.AddSystemInfo("OS", System.Environment.OSVersion.ToString());
            Report.AddSystemInfo("ENV", EnvironmentHelper.EnvironmentType.ToString());
            //report.AddSystemInfo("Browser", $"{DriverProvider.GetDriver()?.Capabilities["browserName"]} {DriverProvider.GetDriver()?.Capabilities["browserVersion"]}");
            Report.AnalysisStrategy = AnalysisStrategy.BDD;
        }
    }
}
