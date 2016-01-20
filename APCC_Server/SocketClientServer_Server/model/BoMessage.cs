using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketClientServer_Server
{
    class BoMessage
    {

        public static String checkSubscription(BoClient client, Boolean ok)
        {
            return "[sub,"+((ok)?1:0)+","+client.id+"]";
        }

        public static String ping(int detailLevel)
        {
            string ret = "[ping;" + Model.singleton.cpuLoad;
            if (detailLevel == 1)
            {
                ret += ";[";
                foreach(BoClient client in Model.singleton.clients)
                {
                    ret += "[" + client.proName + ";" + client.cpuCost + "]";
                }
                ret += "]";
            }
            ret += "]";
            return ret;
        }
        
    }
}
