using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdate_CLI.Classes.Network.API
{
    internal class APIClient
    {
        private static readonly HttpClient client = new HttpClient();
        private ClientConfiguration configuration;

        public APIClient(ClientConfiguration configuration)
        {
            this.configuration = configuration;
        }
    }
}
