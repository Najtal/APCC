using SocketClientServer_Server.process_server;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketClientServer_Server
{

    class MessageParser
    {

        private static char[] delimiterChars = { ';', ':', };

        internal static BoClient newMessage(string message, TcpClient tcpClient)
        {
            Console.WriteLine("recu: " + message);

            Object[] action = getMessageAction(message);
            BoClient client = Model.singleton.clients.Find(x => x.tcp == tcpClient);

            // Si client pas enregistré
            if (client == null)
            {
                switch ((String)action[0])
                {
                    case "sub": // Create new client
                        int priority = Convert.ToInt32(action[2]);
                        int probe = Convert.ToInt32(action[3]);
                        String proName = Convert.ToString(action[4]);
                        String proDescription = Convert.ToString(action[5]);
                        client = Model.singleton.newClient(tcpClient, true, priority, probe, proName, proDescription);
                        Console.WriteLine("nouveau client crée");
                        Server.singleton.sender.sendMessage(client, BoMessage.checkSubscription(client, true));
                        Server.singleton.sender.broadCastMessage("new client" + client.id);
                        break;
                    case "ping": // Ask for cpu load
                        Server.singleton.sender.sendMessage(tcpClient, BoMessage.ping((int)action[1]));
                        break;
                }
            } else
            {
                Console.WriteLine("client : " + client.id);
            }

            return client;
        }

        private static Object[] getMessageAction(string message)
        {
            Object[] data = message.Split(delimiterChars);

            Console.WriteLine("action[]: ");
            foreach (Object o in data)
            {
                Console.WriteLine((string)o + " ; ");
            }

            /*
            // Validate data
            switch ((String)data[0])
            {
                case "sub":
                    if (data.Length != 6) throw new SocketArgumentException("[Error:wrong length]");
                    if ((int)data[1] < 0 || (int)data[1] > 1) throw new SocketArgumentException("[Error:wrong realTime desc]");
                    if ((int)data[2] < 0 || (int)data[2] > 3) throw new SocketArgumentException("[Error:wrong priority]");
                    if ((int)data[2] < 0) throw new SocketArgumentException("[Error:wrong probe]");
                    break;

                case "ping":
                    if (data.Length != 2) throw new SocketArgumentException("[Error:wrong length]");
                    if ((int)data[1] < 0 || (int)data[1] > 1) throw new SocketArgumentException("[Error:wrong detail level]");
                    break;

            }
            */
            return data;
        }

    }
}
