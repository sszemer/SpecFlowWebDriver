using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System;

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
        private static ExtentHtmlReporter htmlReporter;
        private static ExtentKlovReporter klov;
        public static AventStack.ExtentReports.ExtentReports report;
        public static ExtentTest feature;
        public static ExtentTest scenario;
        public static ExtentTest step;

        public static void SetupExtentHtmlReporter()
        {
            htmlReporter = new ExtentHtmlReporter("testReport.html");
            htmlReporter.LoadConfig(configFileName);

            klov = new ExtentKlovReporter();
            klov.InitMongoDbConnection(MongoURL, mongoPort);
            klov.InitKlovServerConnection(KlovURL);

            klov.ProjectName= ReportTitle;
            klov.ReportName= ReportName;

            report = new AventStack.ExtentReports.ExtentReports();
            report.AttachReporter(htmlReporter);
            report.AttachReporter(klov);
            report.AddSystemInfo("OS", System.Environment.OSVersion.ToString());
            //report.AddSystemInfo("Browser", String.Format("{0} {1}", DriverProvider.GetDriver().Capabilities["browserName"], DriverProvider.GetDriver().Capabilities["browserVersion"].ToString()));
            report.AnalysisStrategy = AnalysisStrategy.BDD;
        }
    }
}
