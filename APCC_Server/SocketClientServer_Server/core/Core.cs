using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketClientServer_Server.core
{
    class Core
    {
        private Model model;
        private Server server;

        public Core(Model model, Server server)
        {
            this.model = model;
            this.server = server;
        }

        internal void run()
        {
            while(true)
            {

            }
            //throw new NotImplementedException();
        }
    }
}
