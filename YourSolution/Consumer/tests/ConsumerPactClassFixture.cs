using System;
using Xunit;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace tests
{
    // This class is responsible for setting up a shared
    // mock server for Pact used by all the test
    // Xunit can use a Class Fixture for this.
    public class ConsumerPactClassFixture
    {
        public IPactBuilder PactBuilder { get; private set; }
        public IMockProviderService MockProviderService { get; private set; }

        public int MockServerPort { get { return 9222; } }
        public string MockProviderServiceBaseUri { get { return String.Format("http://localhost:{0}", MockServerPort); } }

        public ConsumerPactClassFixture()
        {
            var pactConfig = new PactConfig
            {
                SpecificationVersion = "2.0.0",
                PactBuilder = @"..\..\..\..\..\pacts",
                LogDir = @".\pact_logs"
            };

            PactBuilder = new PactBuilder(pactConfig);

            PactBuilder.ServiceConsumer("Consumer")
                       .HasPactWith("Provider");

            MockProviderService = PactBuilder.MockService(MockServerPort);
        }

        #region IDisposable Support
        private bool disposedValue = false; //to detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // this will save the pact value
                    PactBuilder.Build();
                }

                disposedValue = true;
            }

        } 

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}