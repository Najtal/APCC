using System;

namespace SocketClientServer_Server
{
    class BoMessage
    {

        public static String checkSubscription(BoClient client, Boolean ok)
        {
            return "[sub,"+((ok)?1:0)+","+client.id+"]";
        }

        public static String ping()
        {
            string ret = "[ping;" + Model.singleton.cpuLoad + "]";
            return ret;
        }

        public static String slowDown()
        {
            return "[action,slowdown]";
        }

        public static String speedUp()
        {
            return "[action,speedup]";
        }

        internal static string givePosition(int lvl)
        {
            return "[action,setScale," + lvl + "]";
        }
    }
}
