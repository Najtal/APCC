using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketClientServer_Server
{
    class BoMessage
    {

        public static String checkSubscription(BoClient client)
        {
            return "[INFO,sub,1,clientId,"+client.id+"]";
        }
        
    }
}
