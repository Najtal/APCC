using SocketClientServer_Server.process_server;
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
        private static Model model;
        public static Server singleton { get; private set; }
        public Sender sender { get; private set; }

        public Server()
        {
            model = Model.singleton;
            Server.singleton = this;
            this.sender = new Sender();
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

                Console.WriteLine("[INFO] [SERVER] Waiting for a connection... ");

                // Enter the listening loop.
                while (true)
                {

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient tcpClient = server.AcceptTcpClient();
                    Console.WriteLine("[INFO] [SERVER] Listening...");

                    // Get a stream object for reading and writing
                    NetworkStream stream = tcpClient.GetStream();

                    int i;
                    data = null;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {

                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Sender.broadCastMessage("[INFO] [SERVER] Received new message: " + data);

                        // Process the data sent by the client.
                        new Task(() => { MessageParser.newMessage(data, tcpClient); }).Start();
                        //BoClient boc = MessageParser.newMessage(data, tcpClient);

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



    }
}
