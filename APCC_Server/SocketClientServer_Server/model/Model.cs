using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Linq;


namespace SocketClientServer_Server
{
    class Model
    {

        public ArrayList clients { get; private set; }
        public Boolean processesHasChange { get; private set; }
        public int marge { get; private set; }
        public int port { get; private set; }
        private int latency;
        private int averageReport;

        public IPAddress adrLocal { get; set; }
        public static Model singleton { get; private set; }

        
        public Model(int marge, int port, int latency, int averageReport)
        {
            if (Model.singleton == null)
                Model.singleton = this;
            else
                throw new Exception();
            
            this.marge = marge;
            this.port = port;
            this.latency = latency;
            this.averageReport = averageReport;
            this.clients = new ArrayList();
            this.processesHasChange = true;
        }

        public BoClient newClient(TcpClient tcp, BoClient.BocType type, int priority, int scale, string proName, string description)
        {
            BoClient nClient = new BoClient(tcp, type, priority, scale, proName, description);
            this.clients.Add(nClient);
            processesHasChange = true;
            return nClient;
        }

        internal ArrayList getProcessNames()
        {
            processesHasChange = false;
            ArrayList processNames = new ArrayList();
            foreach (BoClient client in clients)
                processNames.Add(client.proName);
            return processNames;
        }

        internal void updateClientValues(string[,] newValues, int nbVal)
        {
            for(int i=0;i<nbVal;i++)
            {
                foreach(BoClient boc in clients)
                {
                    if (boc.proName == newValues[i,0])
                    {
                        boc.cpuCost = newValues[i, 1];
                        break;
                    }
                }
            }
            //throw new NotImplementedException();
        }
    }
}
