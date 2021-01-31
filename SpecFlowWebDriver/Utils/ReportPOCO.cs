using AventStack.ExtentReports;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow.Bindings;

namespace SpecFlowWebDriver.Utils
{
    public class ReportPOCO
    {
        public ReportPOCO()
        {
            Features = new List<FeaturePOCO>();
        }
        public List<FeaturePOCO> Features { get; }
    }

    public class FeaturePOCO
    {
        public FeaturePOCO(string title, string description)
        {
            Title = title;
            Description = description;
            Scenarios = new List<ScenarioPOCO>();
        }
        public string Title { get; }
        public string Description { get; }
        public List<ScenarioPOCO> Scenarios { get; }
    }
    public class ScenarioPOCO
    {
        public ScenarioPOCO(string title)
        {
            Title = title;
            Steps = new List<StepPOCO>();
            Categories = new List<string>();
        }
        public string Title { get; }
        public List<string> Categories { get; }
        public List<StepPOCO> Steps { get; }

    }
    public class StepPOCO
    {
        public StepPOCO(string title, StepDefinitionType stepType)
        {
            Title = title;
            StepType = stepType;
        }
        public StepPOCO(string title, StepDefinitionType stepType, Status stepStatus)
        {
            Title = title;
            StepType = stepType;
            StepStatus = stepStatus;
        }
        public string Title { get; }
        public StepDefinitionType StepType { get; }
        public Status StepStatus { get; set; }
        public string URL { get; set; }
        public string PageSource { get; set; }
        public string Screenshot { get; set; }
        public Exception Exception { get; set; }
    }
}
