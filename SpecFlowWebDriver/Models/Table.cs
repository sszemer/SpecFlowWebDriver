using System.Collections.Generic;

namespace SpecFlowWebDriver.Models
{
    public class Rate
    {
        public string No { get; set; }
        public string EffectiveDate { get; set; }
        public double Mid { get; set; }
    }

    public class Table
    {
        public string table { get; set; }
        public string Currency { get; set; }
        public string Code { get; set; }
        public List<Rate> Rates { get; set; }
    }
}
