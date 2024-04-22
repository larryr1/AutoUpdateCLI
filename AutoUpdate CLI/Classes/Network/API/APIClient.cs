using System.Net.Http;

namespace AutoUpdate_CLI.Classes.Network.API
{
    internal class APIClient
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static readonly HttpClient client = new HttpClient();
#pragma warning restore IDE0052 // Remove unread private members
#pragma warning disable IDE0052 // Remove unread private members
        private readonly ClientConfiguration configuration;
#pragma warning restore IDE0052 // Remove unread private members

        internal class REPORT_URLS
        {
            public static string REGISTER = "/api/client/{id}/register";
            public static string STATUS_UPDATE = "/api/client/{id}/update_phase";
            public static string NOTIFY_ = "/api/client/{id}/machine_state";
        }

        public APIClient(ClientConfiguration configuration)
        {
            this.configuration = configuration;
        }
    }
}
