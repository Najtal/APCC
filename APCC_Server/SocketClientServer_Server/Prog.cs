using SocketClientServer_Server.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketClientServer_Server
{
    class Prog
    {

        private Server server;
        private Model model;
        private Core core;

        private static Task tskServer;
        private static Task tskProcessListener;

        // Constructor
        public Prog(int marge, int port, int latency)
        {
            // Init model (Singleton shared entity)
            this.model = new Model(marge, port, latency);

            // Init server (Thread listening to clients)
            tskServer = Task.Factory.StartNew(() => new Server().RunThread());
            Console.WriteLine("[INFO] [PROG] Server listening for clients at: localhost:" + port);
            
            // Init processListener (Thread listening to CPU load)
            tskProcessListener = Task.Factory.StartNew(() => ProcessListener.RunThread(latency));
            Console.WriteLine("[INFO] [PROG] Server ready to measure processes");

            // Init core (Current Thread, scale processes cpu need)
            new Core(latency).run();
            
        }


        public static void Main(String[] args)
        {
            int marge = 20;     // 100%-marge% cpu ou on fait bouger les choses
            int port = 13000;   // port du serveur
            int latency = 100;  // la fréquence (en ms) a laquelle vérifier le cout des process

            new Prog(marge, port, latency);
        }
    }
}
