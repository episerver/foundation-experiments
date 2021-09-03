using EPiServer.Core;
using System.Collections.Generic;

namespace Foundation.Experiments.ExperimentContentArea.Blocks
{
    public class ExperimentVariationResponse
    {
        public ExperimentVariationResponse()
        { }

        public ContentArea ContentArea { get; set; }

        public Dictionary<string, object> Variables { get; set; } = new Dictionary<string, object>();

        public bool Enabled { get; set; }
    }
}