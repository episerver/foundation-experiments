using EPiServer.ServiceLocation;

namespace Foundation.Experiments.Core.Config
{
    [Options]
    public class ExperimentationRestApiOptions
    {
        private string _restAuthToken;
        public string ProjectId { get; set; }

        public string RestAuthToken
        {
            get => _restAuthToken;
            set
            {
                if (value.StartsWith("Bearer "))
                {
                    _restAuthToken = value;
                }
                else
                {
                    _restAuthToken = "Bearer " + value;
                }
            }
        }
    }
}
