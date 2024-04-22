﻿using System.Net;

namespace AutoUpdate_CLI.Classes.Network.API
{
    internal class ClientConfiguration
    {
        private IPEndPoint _serverEndPoint;
        private string _clientIdentifier;
        private string _clientDomain;
        private string _clientHostname;

        public IPEndPoint ServerEndpoint
        {
            get { return _serverEndPoint; }
            set { _serverEndPoint = value; }
        }

        public string ClientIdentifier
        {
            get { return _clientIdentifier; }
            set { _clientIdentifier = value; }
        }

        public string ClientDomain
        {
            get { return _clientDomain; }
            set { _clientDomain = value; }
        }

        public string ClientHostname
        {
            get { return _clientHostname; }
            set { _clientHostname = value; }
        }


    }
}
