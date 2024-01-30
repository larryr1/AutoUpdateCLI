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
        private IPEndPoint _serverEndPoint;
        private string _clientIdentifier;
        private string _clientDomain;

        public IPEndPoint serverEndpoint
        {
            get { return _serverEndPoint; }
            set { _serverEndPoint = value; }
        }

        public string clientIdentifier
        {
            get { return _clientIdentifier; }
            set { _clientIdentifier = value; }
        }

        public string clientDomain
        {
            get { return _clientDomain; }
            set { _clientDomain = value; }
        }
    }
}
