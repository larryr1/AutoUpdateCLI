using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace AutoUpdate_CLI.Classes.Network
{
    /// <summary>
    /// Class that handles the process of discovering an AutoUpdate server.
    /// </summary>
    internal class DiscoveryClient
    {
        private readonly UdpClient client = new UdpClient(29493);

        /// <summary>
        /// Scan for advertisements from an AutoUpdate server and return its IPEndPoint.
        /// </summary>
        /// <returns>IPEndPoint if a valid broadcast is received. null if a valid broadcast is not received.</returns>
        public IPEndPoint DiscoverServer(int timeout)
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            Task<IPEndPoint> discoverTask = Task.Run(Discover, cts.Token);
            Boolean successful = discoverTask.Wait(TimeSpan.FromSeconds(timeout));

            if (!successful)
            {
                cts.Cancel();
                return null;
            }

            return discoverTask.Result;
        }

        /// <summary>
        /// Scan for and parse UDP datagrams that match the AutoUpdate advertisement format.
        /// </summary>
        /// <returns>An IPEndPoint representing the address of the server that was advertised.</returns>
        private IPEndPoint Discover()
        {
            IPEndPoint serverEndpoint = null;

            while (serverEndpoint == null)
            {
                String data = client.Receive(ref serverEndpoint).ToString();
                

                if (TryParseDatagram(data, ref serverEndpoint))
                {
                    serverEndpoint = null;
                    continue;
                }

                return serverEndpoint;
            }

            return null;
        }

        /// <summary>
        /// Attempt to parse an AutoUpdate datagram payload into an IPEndPoint.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="endPoint"></param>
        /// <returns>A boolean representing if the parsing succeeded.</returns>
        private bool TryParseDatagram(String data, ref IPEndPoint endPoint)
        {
            String[] args = data.Split(',');

            if ((args.Length < 3) || (args[0] != "AUADVERTISE"))
            {
                if (args.Length > 1 && args[0] == "AUADVERTISE")
                {
                    Console.WriteLine("Invalid payload sent: " + data);
                }

                return false;
            }


            if (!IPAddress.TryParse(args[1], out IPAddress address))
            {
                Console.WriteLine("Invalid address sent. Payload: " + data);
                return false;
            }

            if (!int.TryParse(args[2], out int port))
            {
                Console.WriteLine("Invalid port number sent. Payload: " + data);
            }

            endPoint.Address = address;
            endPoint.Port = port;
            return true;
        }
    }
}