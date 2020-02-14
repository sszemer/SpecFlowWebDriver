using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace SpecFlowWebDriver.Utils
{
    [Binding]
    public sealed class Hooks
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        [BeforeScenario]
        public void BeforeScenario()
        {
            DriverProvider.GetDriver();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            DriverProvider.CloseDriver();
        }
    }
}
