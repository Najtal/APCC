using System.Net.Sockets;

namespace SocketClientServer_Server
{
    class BoClient
    {

        // STATIC
        private static int idCpt = 0;
        public enum BocType { REAL_TIME, NO_REAL_TIME }

        // VAR
        internal int id;
        internal string cpuCost;

        public TcpClient tcp { get; set; }
        public BocType type { get; private set; }
        public int priority { get; private set; }
        public int scale { get; private set; }
        public string proName { get; private set; }
        public string proDesc { get; private set; }


        public BoClient(TcpClient tcp, BocType type, int priority, int scale, string proName, string proDesc)
        {
            this.id = idCpt;
            idCpt++;

            this.tcp = tcp;
            this.priority = priority;
            this.scale = scale;
            this.proName = proName;
            this.proDesc = proDesc;
        }

    }
}
