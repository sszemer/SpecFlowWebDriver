using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System;
using System.IO;

namespace SpecFlowWebDriver.Utils
{
    public static class Reporter
    {
        private const string ReportTitle = "Test Report";
        private const string ReportName = "SpecFlow Tests";
        private const string KlovURL = "http://127.0.0.1";
        private const string MongoURL = "127.0.0.1";
        private const int mongoPort = 27017;
        private static readonly string configFileName = $"{AppDomain.CurrentDomain.BaseDirectory}..\\..\\..\\Config\\extentReportConfig.xml";
        private static readonly string reportDir = $"{AppDomain.CurrentDomain.BaseDirectory}..\\..\\..\\Report";
        private static ExtentHtmlReporter htmlReporter;
        private static ExtentKlovReporter klov;
        public static AventStack.ExtentReports.ExtentReports report;
        public static ExtentTest feature;
        public static ExtentTest scenario;
        public static ExtentTest step;

        public static void SetupExtentReports()
        {
            InitHtmlReporter(new ExtentHtmlReporter($"{reportDir}\\index.html"));
            InitKlovReporter(new ExtentKlovReporter());
            InitExtentReport(new AventStack.ExtentReports.ExtentReports());
            CleanReportDir(new DirectoryInfo(reportDir));
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
            klov.InitMongoDbConnection(MongoURL, mongoPort);
            klov.InitKlovServerConnection(KlovURL);
            klov.ProjectName = ReportTitle;
            klov.ReportName = ReportName;
        }

        private static void InitExtentReport(AventStack.ExtentReports.ExtentReports extentReports)
        {
            report = extentReports;
            report.AttachReporter(htmlReporter);
            report.AttachReporter(klov);
            report.AddSystemInfo("OS", System.Environment.OSVersion.ToString()); 
            //report.AddSystemInfo("Browser", $"{DriverProvider.GetDriver()?.Capabilities["browserName"]} {DriverProvider.GetDriver()?.Capabilities["browserVersion"]}");
            report.AnalysisStrategy = AnalysisStrategy.BDD;
        }
    }
}
