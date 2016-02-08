using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MultipleThreadConsumptionTest
{
    class ApccLib
    {

        private static String server = "localhost";
        private static int port;
        private static int scale;

        public ApccLib(int p, int s)
        {
            port = p;
            scale = s;

        }

        public void RunThread()
        {

            try
            {
                
                TcpClient client = new TcpClient(server, port);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(getConnexionMessage(scale));

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();
                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                String dataString = null;

                // Receive the TcpServer.response.
                while (true)
                {
                    int i;
                    // Buffer to store the response bytes.
                    data = new Byte[256];

                    // String to store the response ASCII representation.
                    String responseData = String.Empty;

                    while ((i = stream.Read(data, 0, data.Length)) != 0)
                    {

                        // Read the first batch of the TcpServer response bytes.
                        // Int32 bytes = stream.Read(data, 0, data.Length);

                        responseData = System.Text.Encoding.ASCII.GetString(data, 0, i);
                        parseMessage(responseData);

                    }
                }

                // Close everything.
                client.Close();

            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            
        }

        public static String getConnexionMessage(int s)
        {
            String connexionMessage = "sub;0;2;" + s + ";CSharpTest;fcgvhbj";
            return connexionMessage;
        }


        public static void parseMessage(String message)
        {

            Console.WriteLine("received: ", message);
            String[] tokens = message.Split('[');
            tokens = tokens[1].Split(']');
            tokens = tokens[0].Split(',');

            if (tokens[0].Equals("action"))
            {
                if (tokens[1].Equals("setScale"))
                {
                    int newLevel = Convert.ToInt32(tokens[2]);
                    Program.nbZipperLimit = (Program.nbZipperMax + 1 - newLevel);

                    Console.WriteLine("Nb Zipper limit: " +  Program.nbZipperLimit + "/"+ Program.nbZipperMax);
                }
                else if (tokens[1].Equals("speedup"))
                {
                    if (Program.nbZipperLimit < Program.nbZipperMax)
                    {
                        Program.nbZipperLimit++;
                        Console.WriteLine("Nb Zipper limit: " + Program.nbZipperLimit + "/" + Program.nbZipperMax);
                    }
                    
                }
                else if (tokens[1].Equals("slowdown"))
                {
                    if (Program.nbZipperLimit > 1)
                    {
                        Program.nbZipperLimit--;
                        Console.WriteLine("Nb Zipper limit: " + Program.nbZipperLimit + "/" + Program.nbZipperMax);
                    }
                }
            }
        }

    }
}
