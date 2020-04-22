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
        public static ExtentTest step;

        public static void SetupExtentHtmlReporter()
        {
            htmlReporter = new ExtentHtmlReporter("testReport.html");
            htmlReporter.Config.Theme = Theme.Dark;
            htmlReporter.Config.DocumentTitle = "Test Report";
            htmlReporter.Config.ReportName = "SpecFlow Tests";
            report = new AventStack.ExtentReports.ExtentReports();
            report.AttachReporter(htmlReporter);
            report.AddSystemInfo("OS", System.Environment.OSVersion.ToString());

            ExtentKlovReporter klov = new ExtentKlovReporter();
            // specify mongoDb connection
            klov.InitMongoDbConnection("localhost", 27017);

            // specify project
            // ! you must specify a project, other a "Default project will be used"
            klov.ProjectName="projectname";
            // you must specify a reportName otherwise a default timestamp will be used
            klov.ReportName= "AppBuild";
            // URL of the KLOV server
            // you must specify the server URL to ensure all your runtime media is uploaded
            // to the server
            klov.InitKlovServerConnection("http://localhost");
            // finally, attach the reporter
            report.AttachReporter(klov);
        }
    }
}
