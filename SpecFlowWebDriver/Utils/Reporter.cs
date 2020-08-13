using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System;
using System.IO;

namespace SpecFlowWebDriver.Utils
{
    static class Reporter
    {
        private const string ReportTitle = "Test Report";
        private const string ReportName = "SpecFlow Tests";
        private const string KlovURL = "http://localhost";
        private const string MongoURL = "localhost";
        private const int mongoPort = 27017;
        private static string configFileName = $"{AppDomain.CurrentDomain.BaseDirectory}..\\..\\..\\Config\\extentReportConfig.xml";
        private static string reportDir = $"{AppDomain.CurrentDomain.BaseDirectory}..\\..\\..\\Report";
        private static ExtentHtmlReporter htmlReporter;
        private static ExtentKlovReporter klov;
        public static AventStack.ExtentReports.ExtentReports report;
        public static ExtentTest feature;
        public static ExtentTest scenario;
        public static ExtentTest step;

        public static void SetupExtentReports()
        {
            CleanReportDir();
            InitHtmlReporter();
            InitKlovReporter();
            InitExtentReport();
        }

        private static void CleanReportDir()
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(reportDir);
            foreach (FileInfo file in di.GetFiles()) file.Delete();
        }

        private static void InitHtmlReporter()
        {
            htmlReporter = new ExtentHtmlReporter($"{reportDir}\\index.html");
            htmlReporter.LoadConfig(configFileName);
        }
        private static void InitKlovReporter()
        {
            klov = new ExtentKlovReporter();
            klov.InitMongoDbConnection(MongoURL, mongoPort);
            klov.InitKlovServerConnection(KlovURL);
            klov.ProjectName = ReportTitle;
            klov.ReportName = ReportName;
        }

        private static void InitExtentReport()
        {
            report = new AventStack.ExtentReports.ExtentReports();
            report.AttachReporter(htmlReporter);
            report.AttachReporter(klov);
            report.AddSystemInfo("OS", System.Environment.OSVersion.ToString());
            report.AddSystemInfo("Browser", $"{DriverProvider.GetDriver().Capabilities["browserName"]} {DriverProvider.GetDriver().Capabilities["browserVersion"].ToString()}");
            report.AnalysisStrategy = AnalysisStrategy.BDD;
        }
    }
}
