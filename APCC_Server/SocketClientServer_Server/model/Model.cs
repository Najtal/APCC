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
        public static Model singleton { get; private set; }

        // Model data
        public List<BoClient> clients { get; private set; }
        public Boolean processesHasChange { get; private set; }
        public int marge { get; private set; }
        public int port { get; private set; }
        private int latency;
        private int averageReport;

        internal int cpuLoad;
        public IPAddress adrLocal { get; set; }
        
        
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
            this.clients = new List<BoClient>();
            this.processesHasChange = true;
        }

        public BoClient newClient(TcpClient tcp, Boolean type, int priority, int probe, string proName, string proDescription)
        {
            Console.WriteLine("new client 1");
            BoClient nClient = new BoClient(tcp, type, priority, probe, proName, proDescription);
            Console.WriteLine("new client 2");
            this.clients.Add(nClient);
            Console.WriteLine("new client 3");
            processesHasChange = true;
            Console.WriteLine("new client 4");
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


        internal void updateClientValues(Object[,] newValues, int nbtours)
        {
            Console.WriteLine("Consommations :");
            for(int i=0;i< newValues.Length; i++)
            {
                foreach(BoClient boc in clients)
                {
                    if (boc.proName == (String)newValues[i,0])
                    {
                        // on copie la valeur moyenne
                        boc.cpuCost = (int)newValues[i, 1]/nbtours;
                        Console.WriteLine(boc.proName + " id("+boc.id+") , consomme: " + boc.cpuCost); 
                        break;
                    }
                }
            }
        }
    }
}
