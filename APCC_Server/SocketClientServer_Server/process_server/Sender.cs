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

        public static void broadCastMessage(String message)
        {
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);

            // TODO : Handle disconnexions !

            foreach (BoClient cl in Model.singleton.clients)
            {
                sendMessage(cl, msg);
            }
        }

        public static void sendMessage(BoClient client, String message)
        {
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
            sendMessage(client, msg);
        }

        public static void sendMessage(BoClient client, byte[] message)
        {
            sendMessage(client.tcp, message);
        }

        public static void sendMessage(TcpClient tcp, String message)
        {
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
            sendMessage(tcp, msg);
        }

        public static void sendMessage(TcpClient tcp, byte[] message)
        {
            try
            {
                tcp.GetStream().Write(message, 0, message.Length);
            } catch (Exception e)
            {
                Console.WriteLine("[ERROR] [SENDER] error while sending message to " + tcp);
                Model.singleton.clients.Remove(Model.singleton.clients.Find(x => x.tcp == tcp));
            }
            
        }

    }
}
