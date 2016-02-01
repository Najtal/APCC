using System.Net.Sockets;

namespace SocketClientServer_Server
{
    /*
     *  A BoClient is a process that has connected to this server.
     *  The scale the amount of different configuration the process can handle [min 2, max X]
     *  The scalePosition is the position the server asked to client to be
    */

    class BoClient
    {

        // STATIC
        private static int idCpt = 0;
        public enum BocType { REAL_TIME, NO_REAL_TIME }

        // VAR
        internal int id;

        public TcpClient tcp { get; set; }  // The tcp connexion to the client
        public bool type { get; private set; } // true = RealTime ; False = else
        public int priority { get; private set; } // On a scale of 1 to 3, what priority it is. 1 is most prior
        public int scale { get; private set; }  // The size of the option scalability
        public int scalePosition { get; set; }  // [1 -> scale.size], 1 is best full power position
        public string proName { get; private set; }
        public string proDesc { get; private set; }


        public BoClient(TcpClient tcp, bool type, int priority, int scale, string proName, string proDesc)
        {
            this.id = idCpt;
            idCpt++;

            this.tcp = tcp;
            this.priority = priority;
            this.scale = scale;
            this.scalePosition = 1;
            this.proName = proName;
            this.proDesc = proDesc;
        }
        
    }
}
