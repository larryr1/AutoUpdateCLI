using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdate_CLI.Classes.Network.API
{
    internal class ClientConfiguration
    {
        public IPEndPoint serverEndpoint
        {
            get { return serverEndpoint; }
            set { serverEndpoint = value; }
        }

        public string clientIdentifier
        {
            get { return clientIdentifier; }
            set { clientIdentifier = value; }
        }

        public string clientDomain
        {
            get { return clientDomain; }
            set { clientDomain = value; }
        }
    }
}
