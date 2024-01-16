# TCP Chat Application

This is a simple TCP-based chat application consisting of a server and client written in C# using the .NET framework. The communication between the server and clients is facilitated through a custom library (`TCPLibrary`) containing the necessary classes and enums.

## Project Structure

- **TCP_Server:** Contains the server-side implementation.
- **TCP_Client:** Contains the client-side implementation.
- **TCPLibrary:** A library containing shared classes and enums for communication.

## How to Run

1. Open the solution in Visual Studio or your preferred C# development environment.
2. Build the solution to ensure all dependencies are resolved.
3. Run the `TCP_Server` project to start the server.
4. Run one or more instances of the `TCP_Client` project to connect to the server.

## Usage

- Upon connecting, clients can choose commands:
  - `Write`: Send a message to the server.
  - `Read`: Request and display messages from the server.
  - `GetTime`: Request and display the current server time.

- The server processes client requests and broadcasts messages to all connected clients.

## Dependencies

- [Json.NET (Newtonsoft.Json)](https://www.newtonsoft.com/json): Used for JSON serialization and deserialization.

## License

This project is licensed under the [Apache License 2.0](LICENSE), allowing for flexible use, modification, and distribution.

Feel free to contribute or adapt the code for your needs! If you encounter any issues or have suggestions, please open an [issue](https://github.com/yourusername/TCP-Chat/issues).
