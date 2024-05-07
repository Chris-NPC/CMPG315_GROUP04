using ChatServer.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Windows;


namespace ChatServer
{
    class Program
    {
        // Use the  Ctrl + . shortcut to important a namespace
        static List<Client> _users;
        static TcpListener _listener;

        static void Main(string[] args)
        {
            _users = new List<Client>();

            string ipAddress = "192.168.0.21";

            // Public static ip in die client sit
        
            // client.Connect("example.com", 80); // Replace "example.com" with the actual domain name
            // chatappwebsite.duckdns.org

            _listener = new TcpListener(IPAddress.Parse(ipAddress), 80);
            _listener.Start();


            while (true)
            {
                var client = new Client(_listener.AcceptTcpClient());
                // Allow you to add more clients/users to the server
                _users.Add(client);

                /* Broadcast the connection to everyone on the server so that they can update their local list of users. */
                BroadCastConnection();
            }

            //Console.ReadLine();
        }

        static void BroadCastConnection()
        {
            foreach (var user in _users)
            {
                foreach (var usr in _users)
                {
                    var broadcastpacket = new PacketBuilder();
                    broadcastpacket.WriteOpCode(1);
                    broadcastpacket.WriteMessage(usr.Username);
                    broadcastpacket.WriteMessage(usr.UID.ToString());

                    user.ClientSocket.Client.Send(broadcastpacket.GetPacketBytes());
                }
            }
        }

        public static void BroadCastMessage(string message)
        {
            foreach (var user in _users)
            {
                var msgPacket = new PacketBuilder();
                msgPacket.WriteOpCode(5);
                msgPacket.WriteMessage(message);
                user.ClientSocket.Client.Send(msgPacket.GetPacketBytes());
            }
        }

        public static void BroadCastDisconnect(string uid)
        {
            var disconnectedUser = _users.Where(x => x.UID.ToString() == uid).FirstOrDefault();
            _users.Remove(disconnectedUser);

            foreach (var user in _users)
            {
                var broadcastPacket = new PacketBuilder();
                broadcastPacket.WriteOpCode(10);
                broadcastPacket.WriteMessage(uid);
                user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
            }

            BroadCastMessage($"[{disconnectedUser.Username}] Disconnected!");
        }
    }
}
