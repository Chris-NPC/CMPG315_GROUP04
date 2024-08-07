﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Net.IO
{
    // We have data that we want to send to the server
    // Here is where the packet builder is used

    // This class is going to allow us to append data to a memory stream which we will be able to use to get the bytes to send to the server.
    // We are sending the data in the from of bytes from the client to the server.
    class PacketBuilder
    {
        MemoryStream _ms;
        public PacketBuilder()
        {
            _ms = new MemoryStream();
        }

        public void WriteOpCode(byte opcode)
        {
            _ms.WriteByte(opcode);
        }

        public void WriteMessage(string msg)
        {
            var msgBytes = Encoding.ASCII.GetBytes(msg);
            var msgLength = msgBytes.Length;

            // Write the length of the string as a 32-bit integer (4 bytes)
            _ms.Write(BitConverter.GetBytes(msgLength), 0, sizeof(int));

            // Write the string bytes
            _ms.Write(msgBytes, 0, msgBytes.Length);
        }

        public byte[] GetPacketBytes()
        {
            return _ms.ToArray();
        }
    }
}
