using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TCPLibrary
{
    public class UserClass
    {
        public string Name { get; set; }
        public string Message { get; set; }
        public UserCommand Command { get; set; }

        [JsonConstructor]
        public UserClass(string name, UserCommand com)
        {
            Name = name;
            Command = com;
        }

        public UserClass(string name, string message, UserCommand com) : this(name, com)
        {
            Message = message;
        }

    }

    public enum UserCommand
    {
        WriteMes,
        ReadMes,
        GetTime
    }

    public class ServerResponse
    {
        public List<string> ServerMessage { get; set; }
        public DateTime ServerResponseTime { get; set; }

        public ServerResponse(List<string> serverMes)
        {
            ServerMessage = serverMes;
            ServerResponseTime = DateTime.Now;
        }
    }
}
