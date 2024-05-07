using ChatServer.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    class Client
    {
        // Guid = Global Unique Identifier
        // ID of the application in case we want to specify which client we want to do things with a client later on

        public string Username { get; set; }
        public Guid UID { get; set; }

        public TcpClient ClientSocket { get; set; }

        Net.IO.PacketReader _packetReader;

        public Client(TcpClient client)
        {
            ClientSocket = client;
            UID = Guid.NewGuid();
            _packetReader = new Net.IO.PacketReader(ClientSocket.GetStream());

            var opcode = _packetReader.ReadByte();
            Username = _packetReader.ReadMessage();

            // use cw as a shortcut for console writeline
            Console.WriteLine($"[{DateTime.Now}]: Client has connected with the username: {Username}");


            Task.Run(() => Process());

        }

        void Process()
        {
            while (true)
            {
                try
                {
                    var opcode = _packetReader.ReadByte();
                    switch (opcode)
                    {
                        case 5:
                            var msg = _packetReader.ReadMessage();
                            Console.WriteLine($"[{DateTime.Now}]: Message received! {msg}");
                            Program.BroadCastMessage($"[{DateTime.Now}]: [{Username}]: {msg}" );
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception)       // You can throw a NetworkException if you want to
                {
                    Console.WriteLine($"[{UID.ToString()}]: Disconnected!");
                    Program.BroadCastDisconnect(UID.ToString());
                    ClientSocket.Close();
                    break;
                }
            }
        }
    }
}
