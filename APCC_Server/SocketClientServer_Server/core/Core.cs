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

        private static int TIME_TO_SLEEP_WHEN_APOC_RAISED = 500;
        private static int TIME_TO_SLEEP_WHEN_APOC_LOW = 500;

        private List<BoClient> apoClient; // Action Priority Order List ; Client [0] = last to change priority
        private List<BoClient> boclTmp; // temporary apoClient before change insert into apoClient
        private int sleep;
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

            // The main loop
            while (true)
            {

                // Update client list if needed
                boclTmp = model.hasClientListChangeForAPOC();
                if (boclTmp != null)
                {
                    Console.WriteLine("[INFO] [CORE] UPDATE APAC TABLE!");

                    updateApoc(boclTmp);
                    sleep = TIME_TO_SLEEP_WHEN_APOC_LOW;
                }

                // If cpu is too high
                if (model.cpuLimitExceeded)
                {
                    // Core comportement when cpu usage is too high
                    cpuIsTooHigh();
                    sleep = TIME_TO_SLEEP_WHEN_APOC_RAISED;
                    
                }
                // If cpu is low enough
                else if (model.cpuFreeZone)
                {
                    // Core comportement when cpu usage is good
                    cpuIsLow();
                }
                // If cpu usage in good proportion
                else
                {
                    Console.Write(".");
                }


                // Let sleep the required time
                if (sleep == 0) 
                    System.Threading.Thread.Sleep(latency);
                else
                {
                    System.Threading.Thread.Sleep(sleep);
                    sleep = 0;
                }
                
            }

        }

        private void cpuIsTooHigh()
        {
            // go up in APOC
            if (apocPosition > 1)
            {
                Console.WriteLine("apocPosition: " + apocPosition + " / " + apoClient.Count);

                Console.WriteLine("CPU HIGH !");
                BoClient target = apoClient[apocPosition - 1];
                Console.WriteLine("target : " + target.proName);
                Console.WriteLine("target scalePosition : " + target.scalePosition);
                Console.WriteLine("APO size : " + apoClient.Count);
                Console.WriteLine("APO pos : " + apocPosition);
                if (target.scalePosition == target.scale)
                {
                    Console.WriteLine("Target cannot go lower");
                    apocPosition--;
                    cpuIsTooHigh();
                } else
                {
                    Console.WriteLine("ASK target to go Lower: " + target.id);
                    target.scalePosition++;
                    apocPosition--;
                    //broadcastLevel();
                    Sender.sendMessage(target, BoMessage.slowDown());
                }

            }
        }

        private void cpuIsLow()
        {
            // go down in APOC
            if (apocPosition < apoClient.Count)
            {
              
                Console.WriteLine("CPU LOW !");
                BoClient target = apoClient[apocPosition - 1];
                Console.WriteLine("APO : " + apocPosition + "/" + apoClient.Count);

                if (target.scalePosition == 1)
                {
                    Console.WriteLine("Target cannot go faster");
                    apocPosition++;
                    cpuIsLow();
                }
                else
                {
                    Console.WriteLine("ASK target to go Faster: " + target.id);
                    target.scalePosition--;
                    apocPosition++;
                    //broadcastLevel();
                    Sender.sendMessage(target, BoMessage.speedUp());
                }

                Console.WriteLine("APO : " + apocPosition + "/" + apoClient.Count);

            }
        }


        private void updateApoc(List<BoClient> boclTmp)
        {

            Console.WriteLine("Boc list Tmp description");
            foreach(BoClient b in  boclTmp)
            {
                Console.WriteLine("BOC: " + b.proName);
            }

            // Init of the calc list
            boclTmp = boclTmp.OrderBy(o => (o.priority*5 + o.scale)).ToList();

            if (boclTmp.Count < apocPosition)
            {
                apocPosition = boclTmp.Count;
            }

            apoClient = new List<BoClient>();
            foreach(BoClient boc in boclTmp)
            {
                for(int i=0; i<boc.scale;i++)
                {
                    apoClient.Add(boc);
                    Console.WriteLine("######## boc pos: " + i + " -> id: " + boc.id);
                }
            }

            apocPosition = apoClient.Count;

            // Update BOC positions
            /*for (int i = 0; i < apocPosition; i++)
            {
                apoClient[i].scalePosition = 1;
            }
            
            for (int i = apocPosition ; i<apoClient.Count-1; i++)
            {
                apoClient[i].scalePosition++;
            }*/


            // Share new position
            broadcastLevel();

        }

        private void broadcastLevel() {
            foreach (BoClient boc in apoClient)
            {
                Sender.sendMessage(boc, BoMessage.givePosition(boc.scalePosition));
            }
        }

    }
}
