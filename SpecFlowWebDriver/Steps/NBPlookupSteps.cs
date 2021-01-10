using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using System.Text.Json;

namespace SpecFlowWebDriver
{
    [Binding, Parallelizable]
    public class NBPlookupSteps
    {
        private readonly HttpClient client;
        private Models.Table respBody;
        private readonly ScenarioContext scenarioContext;

        public NBPlookupSteps(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
            client = new HttpClient();
        }

        [Given(@"NBP rest api is online")]
        public async Task GivenNBPRestApiIsOnlineAsync()
        {
            HttpResponseMessage response = await client.GetAsync("http://api.nbp.pl/");
            Assert.IsTrue(response.IsSuccessStatusCode);
        }
        
        [When(@"I lookup the currency for (.*)")]
        public async Task WhenILookupTheCurrencyForAsync(string p0)
        {
            HttpResponseMessage response = await client.GetAsync(String.Format("http://api.nbp.pl/api/exchangerates/rates/a/{0}/last/1/?format=json", p0));
            respBody = JsonSerializer.Deserialize<Models.Table>(await response.Content.ReadAsStringAsync());
            Assert.IsNotNull(respBody.rates[0].mid);
        }
        
        [Then(@"I want to know if the rate is below (.*)")]
        public void ThenIWantToKnowIfTheRateIsBelow(Double p0)
        {
            Assert.Less(respBody.rates[0].mid, p0);
        }
    }
}
