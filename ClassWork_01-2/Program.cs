using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Libraries
using System.Net;
using System.Net.Sockets;
using System.Threading;



// Client Programm
namespace ClassWork_01_2
{
    internal class Program
    {
        static Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); 

        static void Main(string[] args)
        {
            Console.Write("Enter server ip: ");
            string IpServer = Console.ReadLine();
            Console.Write("Enter server port");
            int port = int.Parse(Console.ReadLine());

            try
            {
                // 1) Client - Server connection
                client.Connect(IpServer, port);
                
                // 2) Check connection
                if (client.Connected == false)
                {
                    throw new Exception("No connection with server");
                }
                Console.WriteLine("Success - Connected with server");
                
                // 3) Server ==> "Hello"
                byte[] buffer = new byte[1024];
                int size = client.Receive(buffer);
                Console.WriteLine($"Message from server [{size}]: {Encoding.UTF8.GetString(buffer, 0, size)}");
                
                // 4) UserName ==> Server
                string UserName = "Abylaikhan";
                client.Send(Encoding.UTF8.GetBytes(UserName));
                Console.WriteLine($"Name {UserName} send to server");

                // 5) 
                while (true)
                {
                    // 5.1)
                    Console.WriteLine("Enter messages to server");
                    string msg = Console.ReadLine();
                    if (msg == "") { continue; }
                    client.Send(Encoding.UTF8.GetBytes(msg));
                    // 5.2)
                    size = client.Receive(buffer);
                    string temp = Encoding.UTF8.GetString(buffer, 0, size);
                    Console.WriteLine($"Server message[{size}]: {temp}");

                    if (msg == "Exit")
                    {
                        size = client.Receive(buffer);
                        temp = Encoding.UTF8.GetString(buffer, 0, size);
                        Console.WriteLine($"Server message[{size}]: {temp}");
                        client.Disconnect(true); // Disconnect boolean true/false answers
                                                 // For this question, "Использовать ли сокет повторно"
                                                 // True - Да, не уничтожать
                                                 // False - Нет, разорвать связь тоже самое что и client.Close();
                        client.Close();
                        break;
                    }
                } // while (true)
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error - {ex.Message}");
            } // try-catch
            Console.WriteLine("Client exit...");
            Console.ReadLine();

        } // static void Main()
    } // internal class Program
} // namespace ClassWork_01_2
