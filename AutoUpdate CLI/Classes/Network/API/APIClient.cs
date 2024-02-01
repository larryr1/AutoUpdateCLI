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

        public APIClient(ClientConfiguration configuration)
        {
            this.configuration = configuration;
        }
    }
}
