using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TCPLibrary;


namespace TCP_Server
{
    class TCPServer
    {
        static void Main()
        {
            var messageList = new List<string>();

            // Set the IP address and port for listening
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            int port = 8888;

            // Create a TCP server socket
            TcpListener serverSocket = new TcpListener(ipAddress, port);
            serverSocket.Start();

            Console.WriteLine($"Server is running. Waiting for connections on {ipAddress}:{port}");

            while (true)
            {
                Console.WriteLine("Waiting client");
                // Accept the client socket
                TcpClient clientSocket = serverSocket.AcceptTcpClient();

                Console.WriteLine($"Client connected: {((IPEndPoint)clientSocket.Client.RemoteEndPoint).Address}");

                // Handle the connected client in a separate thread
                HandleClient(clientSocket, messageList);
            }
        }

        static void HandleClient(TcpClient clientSocket, List<string> messages)
        {
            try
            {
                // Get the stream for reading and writing data
                NetworkStream networkStream = clientSocket.GetStream();

                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = networkStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    // Convert the received data to a UserClass
                    string dataFromClient = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                    UserClass userClass = JsonConvert.DeserializeObject<UserClass>(dataFromClient);

                    // Send a response to the client
                    HandlerUserCommand(userClass, networkStream, messages);

                }

                // Close the connection
                clientSocket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling the client: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Client connetion is close!");
            }
        }

        static void HandlerUserCommand(UserClass userClass, NetworkStream networkStream, List<string> messages)
        {
            UserCommand userCommand = userClass.Command;

            if (userCommand == UserCommand.WriteMes)
            {
                // Add data message to messageList
                messages.Add($"{userClass.Name}: {userClass.Message}");

                SendToUser(networkStream, new ServerResponse(messages));
            }
            else if (userCommand == UserCommand.ReadMes)
            {
                SendToUser(networkStream, new ServerResponse(messages));
            }
            else if (userCommand == UserCommand.GetTime)
            {
                SendToUser(networkStream, new ServerResponse(new List<string> { DateTime.Now.ToString("f") }));
            }

        }

        static void SendToUser(NetworkStream networkStream, ServerResponse serverResponse)
        {
            // Serialize data
            string dataJson = JsonConvert.SerializeObject(serverResponse);

            // Write data to Network
            byte[] dataToSend = Encoding.Unicode.GetBytes(dataJson);
            networkStream.Write(dataToSend, 0, dataToSend.Length);
            networkStream.Flush();
        }

    }
}

