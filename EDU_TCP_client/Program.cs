using System;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using EDU_TCP_client;

namespace TcpClient
{
    class Program
    {
        static void Main()
        {
            string serverIP = "127.0.0.1";
            int serverPort = 8888;

            TcpClient clientSocket = new TcpClient();

            try
            {
                clientSocket.Connect(serverIP, serverPort);
                Console.WriteLine($"Подключено к серверу {serverIP}:{serverPort}");

                NetworkStream networkStream = clientSocket.GetStream();

                // Создаем объект UserClass для отправки
                UserClass user = new UserClass("John Doe", "Some user information");

                // Сериализуем объект в JSON
                string dataToSend = JsonConvert.SerializeObject(user);

                // Отправляем данные серверу
                byte[] dataBytes = Encoding.Unicode.GetBytes(dataToSend);
                networkStream.Write(dataBytes, 0, dataBytes.Length);
                networkStream.Flush();

                // Получаем ответ от сервера
                byte[] buffer = new byte[1024];
                int bytesRead = networkStream.Read(buffer, 0, buffer.Length);
                string responseFromServer = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Ответ от сервера: {responseFromServer}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при подключении к серверу: {ex.Message}");
            }
            finally
            {
                if (clientSocket != null && clientSocket.Connected)
                    clientSocket.Close();
            }
        }
    }
}
