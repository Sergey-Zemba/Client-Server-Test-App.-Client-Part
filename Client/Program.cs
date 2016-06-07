using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Program
    {
        private static int port = Int32.Parse(ConfigurationManager.AppSettings["port"]);
        private static string address = ConfigurationManager.AppSettings["address"];
        private static void Main(string[] args)
        {
            Console.WriteLine("Input your name: ");
            string user = Console.ReadLine();
            TcpClient client = null;
            try
            {
                client = new TcpClient(address, port);
                NetworkStream stream = client.GetStream();
                while (true)
                {
                    Console.WriteLine(user + ": ");
                    string message = Console.ReadLine();
                    message = String.Format("{0}: {1}", user, message);
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                    data = new byte[64];
                    StringBuilder response = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        response.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    } while (stream.DataAvailable);
                    message = response.ToString();
                    Console.WriteLine("Server: {0}", message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Close();
            }
        }
    }
}
