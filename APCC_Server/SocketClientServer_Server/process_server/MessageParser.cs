using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketClientServer_Server
{
    class MessageParser
    {
        internal static BoClient newMessage(string message, TcpClient tcpClient)
        {
                       
            //throw new NotImplementedException();

            // SI PARSE NOUVEAU CLIENT -> creer nouvea client
            BoClient nClient = Model.singleton.newClient(tcpClient, BoClient.BocType.NO_REAL_TIME, 1, 2, "test", "description");

            return nClient;
        }
    }
}
