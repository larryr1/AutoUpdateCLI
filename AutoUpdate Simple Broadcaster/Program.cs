using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AutoUpdate_Simple_Broadcaster
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("AutoUpdate Simple Broadcaster");

            IPAddress? address = null;
            while (address == null)
            {
                Console.WriteLine("Enter the configuration server IP address.");
                try
                {
                    address = IPAddress.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("That is not a valid IP address.");
                    address = null;
                }
            }

            Console.WriteLine("Using IP address " + address.ToString());

            int port = -1;
            while (port == -1)
            {
                Console.WriteLine("Enter the configuration server port.");
                try
                {
                    port = int.Parse(Console.ReadLine());
                    if (port < 1 || port > 65535) throw new ArgumentOutOfRangeException();
                }
                catch (Exception)
                {
                    Console.WriteLine("That is not a valid port number.");
                    port = -1;
                }
            }

            Console.WriteLine("Using port number " + port.ToString());

            string dataString = $"AUADVERTISE,{address},{port}";

            Console.WriteLine($"Broadcasting the following string over UDP on port 29463: \"{dataString}\"");
            Console.WriteLine("Press CTRL+C at any time to stop.");

            UdpClient client = new UdpClient();
            client.EnableBroadcast = true;
            client.Client.Bind(new IPEndPoint(IPAddress.Any, port));

            byte[] payload = Encoding.ASCII.GetBytes(dataString);

            Console.WriteLine("Broadcast adddress is " + IPAddress.Broadcast.ToString());

            Task task = Task.Run(() =>
            {
                while (true)
                {
                    client.Send(payload, payload.Length, new IPEndPoint(IPAddress.Broadcast, 29463));
                    Console.WriteLine("Broadcasted.");
                    Thread.Sleep(1000);
                }
            }); 

            task.Wait();


            Console.ReadLine();
        }
    }
}
