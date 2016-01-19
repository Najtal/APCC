using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketClientServer_Server
{
    class Server
    {
        private Model model;

        public Server()
        {
            this.model = Model.singleton;
        }

        public void RunThread()
        {
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = model.port;
                model.adrLocal = IPAddress.Parse("127.0.0.1");
                // model.adrLocal = Dns.GetHostEntry("localhost").AddressList[1];
                IPAddress localAddr = model.adrLocal;

                // TcpListener server = new TcpListener(port);
                TcpListener server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                Console.Write("Waiting for a connection... ");

                // Enter the listening loop.
                while (true)
                {

                    Console.WriteLine("Ready for new client");

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient tcpClient = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    // Get a stream object for reading and writing
                    NetworkStream stream = tcpClient.GetStream();

                    int i;
                    data = null;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {

                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine(String.Format("Received: {0}", data));

                        // Process the data sent by the client.
                        BoClient boc = MessageParser.newMessage(data, tcpClient);
                       
                        sendMessage(boc, BoMessage.checkSubscription(boc));

                        broadCastMessage("On a recu un message ici ;)");

                        break;
                    }

                    // Shutdown and end connection
                    //client.Close();
                }

            }

            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }

        public void broadCastMessage(String message)
        {
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
            foreach (BoClient cl in model.clients)
            {
                sendMessage(cl, msg);
            }
            Console.WriteLine(String.Format("Broadcasté à {0} clients", model.clients.Count));
        }

        public void sendMessage(BoClient client, String message)
        {
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
            sendMessage(client, msg);
            Console.WriteLine(String.Format("Envoyé à client {0}", client.id));
        }

        private void sendMessage(BoClient client, byte[] message)
        {
            client.tcp.GetStream().Write(message, 0, message.Length);
        }

    }
}
