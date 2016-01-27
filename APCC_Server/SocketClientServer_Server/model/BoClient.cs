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

        public TcpClient tcp { get; set; }
        public bool type { get; private set; }
        public int priority { get; private set; }
        public int scale { get; private set; }
        public int scalePosition { get; set; }
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
