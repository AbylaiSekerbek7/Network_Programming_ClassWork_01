using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Libraries
using System.Net;
using System.Net.Sockets;
using System.Threading;



// Server Programm
namespace ClassWork_01
{
    internal class Program
    {
        // most popular protocols IP/TCP = IP - "InterNetwork", TCP - "Tcp";
        static Socket socServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        static string ipAddress = "0.0.0.0"; // localchost, local Machine
                                               // 0.0.0.0 - Маска для всех компьютеров
                                               // 192.168.1.10 - Локальная сеть
        static int port = 12345; // server port, for connection
        static void Main(string[] args)
        {
            // Конечная точка сервера
            EndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            // Привязка сервера с EndPoint-ом
            socServer.Bind(serverEndPoint);
            Console.WriteLine($"Bind: {serverEndPoint}");
            // Запускаем сокет на прослушивание сети для подключения клиента
            // 100 - Глубина очереди ожидаемых клиентов
            socServer.Listen(100);
            Console.WriteLine("Listening started - waitining Client");

            // 1) Accept - Ожидание подключения клиента
            Socket client = socServer.Accept(); // в client - Будет наш клиент
            Console.WriteLine($"Client is connected: {client.RemoteEndPoint}");
            
            // 2) Hello Client
            // Кодировка
            // UTF8 - обычно 1 символ = 1 byte
            // Unicode - обычно 1 символ = 2 byte
            // ASCII - лучше всего работает с En, Ru языками. Поддержку с Kz нету, мест нету
            //byte[] buffer = Encoding.UTF8.GetBytes(ipAddress);
            client.Send(Encoding.UTF8.GetBytes(ipAddress));

            // 3) Receive message from client - имя клиента
            byte[] bufferReceive = new byte[1024]; // buffer to get message
            // if size = 0 Разорвана сеть
            // if size < 0 Что то случилось - Ошибка - Исключение
            // if size > 0 && size <= 1024 
            int size = client.Receive(bufferReceive); // size of the message 
            string msg = Encoding.UTF8.GetString(bufferReceive, 0, size);

            Console.WriteLine($"Message Accepted! size: {size} byte");
            Console.WriteLine($"Client Message: {msg}");

            // 4) 
            while (true)
            {
                // 4.1)
                size = client.Receive(bufferReceive);
                msg = Encoding.UTF8.GetString(bufferReceive, 0, size);
                Console.WriteLine($"Message Accepted! size: {size} byte");
                Console.WriteLine($"Message: {msg}");
                // 4.2) Подтверждение от сервера о получении сообщения
                client.Send(Encoding.UTF8.GetBytes("OK"));
                if (msg == "Exit")
                {
                    client.Send(Encoding.UTF8.GetBytes("Good bye client...."));
                    client.Close(); // stop connection with client
                    break;
                }
            } // while (true)

            Console.WriteLine("Server exit...");
            Console.ReadLine();
        } // static void Main(string[] args)
    } // internal class Programm
} // namespace ClassWork_01