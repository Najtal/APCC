using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketClientServer_Server.process_server
{
    class Sender
    {

        public void broadCastMessage(String message)
        {
            Console.WriteLine("Start broadcast: " + message);
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
            foreach (BoClient cl in Model.singleton.clients)
            {
                sendMessage(cl, msg);
            }
            Console.WriteLine(String.Format("Broadcasté à {0} clients", Model.singleton.clients.Count));
        }

        public void sendMessage(BoClient client, String message)
        {
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
            sendMessage(client, msg);
            Console.WriteLine(String.Format("Envoyé à client {0}", client.id));
        }

        public void sendMessage(BoClient client, byte[] message)
        {
            sendMessage(client.tcp, message);
        }

        public void sendMessage(TcpClient tcp, String message)
        {
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
            sendMessage(tcp, msg);
        }

        public void sendMessage(TcpClient tcp, byte[] message)
        {
            Console.WriteLine("Send message: " + message);
            tcp.GetStream().Write(message, 0, message.Length);
        }

    }
}
