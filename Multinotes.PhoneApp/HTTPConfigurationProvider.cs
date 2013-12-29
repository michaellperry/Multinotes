using System.Linq;
using UpdateControls.Fields;
using UpdateControls.Correspondence.BinaryHTTPClient;
using Multinotes.Model;

namespace Multinotes.PhoneApp
{
    public class HTTPConfigurationProvider : IHTTPConfigurationProvider
    {
        private Independent<Individual> _individual = new Independent<Individual>();

        public Individual Individual
        {
            get { return _individual; }
            set { _individual.Value = value; }
        }

        public HTTPConfiguration Configuration
        {
            get
            {
                string address = "https://api.facetedworlds.com/correspondence_server_web/bin";
                string apiKey = "D1920D309A4E43EB85BD14833AFEF7E8";
                return new HTTPConfiguration(address, "Multinotes.PhoneApp", apiKey);
            }
        }

        public bool IsToastEnabled
        {
            get { return false; }
        }
    }
}
