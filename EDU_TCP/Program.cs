using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using EDU_TCP;

namespace TcpServer
{
    class Program
    {
        static void Main()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            int port = 8888;

            TcpListener serverSocket = new TcpListener(ipAddress, port);
            serverSocket.Start();

            Console.WriteLine($"Сервер запущен. Ожидание подключений на {ipAddress}:{port}");

            while (true)
            {
                TcpClient clientSocket = serverSocket.AcceptTcpClient();
                Console.WriteLine($"Клиент подключен: {((IPEndPoint)clientSocket.Client.RemoteEndPoint).Address}");

                // Обработка подключенного клиента в отдельном потоке
                HandleClient(clientSocket);
            }
        }

        static void HandleClient(TcpClient clientSocket)
        {
            try
            {
                NetworkStream networkStream = clientSocket.GetStream();

                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = networkStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string dataFromClient = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Получено от клиента: {dataFromClient}");

                    // Десериализуем данные из JSON в объект
                    UserClass receivedUser = JsonConvert.DeserializeObject<UserClass>(dataFromClient);

                    // Получаем значения полей объекта
                    string name = receivedUser.Name;
                    string info = receivedUser.Info;

                    Console.WriteLine($"Данные: Имя={name}, Информация={info}");

                    // Отправляем ответ клиенту
                    string responseMessage = $"Сервер получил данные: {dataFromClient}";
                    byte[] responseBytes = Encoding.Unicode.GetBytes(responseMessage);
                    networkStream.Write(responseBytes, 0, responseBytes.Length);
                    networkStream.Flush();
                }

                // Закрываем соединение
                clientSocket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка обработки клиента: {ex.Message}");
            }
        }
    }
}
