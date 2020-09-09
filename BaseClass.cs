
namespace SocioPress
{
    public class SPHost
    {
        private static SPHost instance;
        public static SPHost Instance
        {
            get
            {
                if (instance == null)
                    instance = new SPHost();
                return instance;
            }
        }

        private bool isInitialized = false;
        private string baseUrl = "http://localhost";
        public string BaseDomain
        {
            get
            {
                return baseUrl + "/wp-json";
            }
        }

        public void Initialized(string url)
        {
            if (!isInitialized)
            {
                baseUrl = url;
                isInitialized = true;
            }
        }

    }
}
