using System;

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
            string ret = "[ping;" + Model.singleton.cpuLoad + "]";
            return ret;
        }
        
    }
}
