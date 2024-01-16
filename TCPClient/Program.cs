using System;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using TCPLibrary;


namespace TCP_Client
{
    class TCPClient
    {
        static void Main()
        {

            // Set the IP address and port of the server
            string serverIP = "127.0.0.1";
            int serverPort = 8888;

            UserClass userClass = ReadUserInfo();

            while (true)
            {
                // Create a TCP client socket
                TcpClient clientSocket = new TcpClient();

                try
                {
                    // Connect to the server
                    clientSocket.Connect(serverIP, serverPort);
                    Console.WriteLine($"Connected to the server {serverIP}:{serverPort}");

                    // Get the stream for reading and writing data
                    NetworkStream networkStream = clientSocket.GetStream();

                    // Serialize UserClass
                    string dataJson = JsonConvert.SerializeObject(userClass);
                    
                    // Send data to the server
                    byte[] dataToSend = Encoding.Unicode.GetBytes(dataJson);
                    networkStream.Write(dataToSend, 0, dataToSend.Length);
                    networkStream.Flush();

                    GetDataFormServer(networkStream);

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error connecting to the server: {ex.Message}");
                }
                finally
                {
                    //Close the client socket
                    if (clientSocket != null && clientSocket.Connected)
                        clientSocket.Close();
                }

                userClass = ReadUserInfo();
                Console.Clear();
            }

        }

        static UserClass ReadUserInfo()
        {
            Console.WriteLine("Enter youre Name:");
            string userName = Console.ReadLine();
            Console.WriteLine("Choose command: TimeNow/Write/Read");
            string userCommand = Console.ReadLine();

            if (userCommand == "Write")
            {
                Console.WriteLine("Enter youre message:");
                string userMessage = Console.ReadLine();
                return new UserClass(userName, userMessage, UserCommand.WriteMes);
            }
            else if (userCommand == "Read")
            {
                return new UserClass(userName, UserCommand.ReadMes);
            }
            else { return new UserClass(userName, UserCommand.GetTime); }
        }

        static void GetDataFormServer(NetworkStream networkStream)
        {
            // Receive a response from the server
            byte[] buffer = new byte[10240];
            int bytesRead = networkStream.Read(buffer, 0, buffer.Length);
            string responseFromServer = Encoding.Unicode.GetString(buffer, 0, bytesRead);

            // Deserialize data
            ServerResponse serverResponse = JsonConvert.DeserializeObject<ServerResponse>(responseFromServer);

            // Write server response
            Console.WriteLine($"===Data getеing form server at {serverResponse.ServerResponseTime}===");
            foreach (var message in serverResponse.ServerMessage)
            {
                Console.WriteLine($"--{message}--");
            }
        }

    }
}

