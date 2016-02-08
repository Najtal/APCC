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

                BoClient target = apoClient[apocPosition - 1];
                if (target.scalePosition == target.scale)
                {
                    apocPosition--;
                    cpuIsTooHigh();
                } else
                {
                    target.scalePosition++;
                    apocPosition--;
                    //broadcastLevel();
                    Sender.sendMessage(target, BoMessage.slowDown());
                }

                Console.WriteLine("apocPosition: " + apocPosition + " / " + apoClient.Count);
            }
        }

        private void cpuIsLow()
        {
            // go down in APOC
            if (apocPosition < apoClient.Count)
            {
              
                BoClient target = apoClient[apocPosition - 1];

                if (target.scalePosition == 1)
                {
                    apocPosition++;
                    cpuIsLow();
                }
                else
                {
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

            Console.WriteLine("# clients: " + boclTmp.Count);
            

            apoClient = new List<BoClient>();
            for (int i=1; i<=3; i++)
            {
                List<BoClient> blPriority = boclTmp.Where(p => p.priority == i).OrderBy(o => o.scale).ToList();

                if (blPriority.Count > 0) { 
                    int[] tabScale = new int[blPriority.Count];
                    for (int j = 0; j < tabScale.Length; j++)
                    {
                        tabScale[j] = blPriority[j].scale;
                    }

                    int sumScale = blPriority.Sum(p => p.scale);
                    Console.WriteLine("sum scale priorité " + i + ": " + sumScale);
                    int bloPos = 0;
                    do
                    {
                        if (tabScale[bloPos] > 0)
                        {
                            apoClient.Add(blPriority[bloPos]);
                            sumScale--;
                        }
                        bloPos++;
                        if (bloPos == tabScale.Length)
                            bloPos = 0;
                    } while (sumScale != 0);
                }
            }

            apocPosition = apoClient.Count;

            foreach (BoClient b in apoClient)
            {
                b.scalePosition = 1;
            }

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
