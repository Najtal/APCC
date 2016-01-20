using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace SocketClientServer_Server
{
    internal static class ProcessListener
    {

        private static Model model = Model.singleton;
        private static ArrayList processName;
        private static int latency; // la fréquence (en ms) a laquelle vérifier le cout des process
        private static int averageReport; // remonter au modele les données tout les x latency

        private static int counter; // compteur tours (1er boucle)
        private static int nvCpt; // process check counter (2nd boucle)

        
        internal static void RunThread(int latency, int averageReport)
        {
            ProcessListener.latency = latency;
            ProcessListener.averageReport = averageReport;
            Object[,] newValues = null; // local model [thread_name , avg_cpu_usage]
            nvCpt = 0;

            // DEBUT BOUCLE TOUR
            while (true)
            {
                
                counter++; // On incrémente le nombre de tours (1er boucle)
                nvCpt = 0; // Compteur de process check (2e boucle)
                
                // Si les clients enregistrés ont changés, on les réactualisent
                if (model.processesHasChange)
                {
                    processName = model.getProcessNames();
                    counter = 0;
                }

                // si on est dans un tour ou il faut partager
                if (counter == 0)
                    newValues = new Object[processName.Count, 2];


                // DEBUT BOUCLE PROCESS
                Process[] runningNow = Process.GetProcesses();
                foreach (Process process in runningNow.Where(x => processName.Contains(x)))
                {
                    using (PerformanceCounter pcProcess = new PerformanceCounter("Process", "% Processor Time", process.ProcessName))
                    //using (PerformanceCounter memProcess = new PerformanceCounter("Memory", "Available MBytes"))
                    {
                        pcProcess.NextValue();

                        float cpuUseage = pcProcess.NextValue();
                        //Console.WriteLine("Process: '{0}' CPU Usage: {1}%", process.ProcessName, cpuUseage);
                        //float memUseage = memProcess.NextValue();
                        //Console.WriteLine("Process: '{0}' RAM Free: {1}MB", process.ProcessName, memUseage);

                        newValues[nvCpt,0] += Convert.ToString(processName[nvCpt]);
                        newValues[nvCpt, 1] = (newValues[nvCpt, 1]==null) ? cpuUseage : ((int)newValues[nvCpt, 1])+cpuUseage;

                        nvCpt++; // incremente tour
                    }
                }

                if (counter == ProcessListener.averageReport)
                {
                    model.updateClientValues(newValues, counter);
                    counter = 0;
                }


                // Compute overall cpu load
                PerformanceCounter cpuCounter = new PerformanceCounter();
                cpuCounter.CategoryName = "Processor";
                cpuCounter.CounterName = "% Processor Time";
                cpuCounter.InstanceName = "_Total";
                model.cpuLoad = (int)cpuCounter.NextValue();

                Thread.Sleep(ProcessListener.latency);

            }

        }
    }
}