using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ClientTDP
{
    class Program
    {
        static void Main(string[] args)
        {
            const string ip = "127.0.0.1";
            const int port = 8082;

            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(tcpEndPoint);

            while (true)
            {
                Console.WriteLine("Введите сообщение: ");
                string message = Console.ReadLine();

                var endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8082);
                socket.SendTo(Encoding.UTF8.GetBytes(message), endPoint);

                var buffer = new byte[256];
                var size = 0;
                var data = new StringBuilder();
                EndPoint lideneEndPoint = new IPEndPoint(IPAddress.Any, 0);

                do
                {
                    size = socket.ReceiveFrom(buffer, ref lideneEndPoint);
                    data.Append(Encoding.UTF8.GetString(buffer));
                } 
                while (socket.Available > 0);

                socket.SendTo(Encoding.UTF8.GetBytes($"Сообщение получено! "), lideneEndPoint);
                Console.WriteLine(data);
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }
    }
}
