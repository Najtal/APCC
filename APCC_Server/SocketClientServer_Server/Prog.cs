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
        
        // Constructor
        public Prog(int marge, int port, int latency, int averageReport)
        {
            // On génère le modèle
            this.model = new Model(marge, port, latency, averageReport);
            Task tskServer = Task.Factory.StartNew(() => new Server().RunThread());
            Console.WriteLine("Server listening for clients at: localhost:" + port);
            Task tskProcessListener = Task.Factory.StartNew(() => ProcessListener.RunThread(latency, averageReport));
            Console.WriteLine("Server ready to measure processes");

            while (true) {}
            //this.server = Task.Factory.StartNew();
            //this.server = new Server(this.model);
        }

        public static void Main(String[] args)
        {
            int marge = 20;
            int port = 13000;
            int latency = 250;
            int averageReport = 4;

            new Prog(marge, port, latency, averageReport);
        }
    }
}
