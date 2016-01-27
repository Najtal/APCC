using SocketClientServer_Server.process_server;
using System;
using System.Collections;
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

        private int clientsState; // SUM ( client.scale ) in client
        private List<BoClient> apoClient; // Action Priority Order Client
        private int apocPosition;
        private int latency;

        public Core(int latency)
        {
            this.model = Model.singleton;
            this.server = Server.singleton;
            this.apoClient = new List<BoClient>();
            this.apocPosition = 0;
            this.latency = latency;
        }

        internal void run()
        {

            initApoc();
            List<BoClient> boclTmp;

            // The main loop
            while (true)
            {

                // Update client list if needed
                boclTmp = model.hasClientListChangeForAPOC();
                if (boclTmp != null)
                {
                    updateApoc(boclTmp);
                }

                // If cpu is too high
                if (model.cpuLimitExceeded)
                {
                    // Core comportement when cpu usage is too high
                    cpuIsTooHigh();

                    System.Threading.Thread.Sleep(500);
                    model.cpuLimitExceeded = false;

                }

                // If cpu is low enough
                if (model.cpuLoad < (100-model.marge-20))
                {
                    // Core comportement when cpu usage is good
                    //cpuIsLow();
                }
            }

            System.Threading.Thread.Sleep(latency);
        }

        private void cpuIsLow()
        {
            // TODO : go up in APOC
            Sender.broadCastMessage("cpu is low" + model.cpuLoad + ", feel free to use it");
        }

        private void cpuIsTooHigh()
        {
            // TODO : go down in APOC
            Sender.broadCastMessage("cpu is too high:" + model.cpuLoad + ", you have to slow down");   
        }

        private void updateApoc(List<BoClient> boclTmp)
        {
            // TODO : modification in client list : APOC must be updated
        }

        private void initApoc()
        {
            // TODO : set APOC list
        }
    }
}
