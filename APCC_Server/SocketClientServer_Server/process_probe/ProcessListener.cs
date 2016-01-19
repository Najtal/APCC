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
        private static int latency;
        private static int counter;
        private static int averageReport;

        internal static void RunThread(int latency, int averageReport)
        {
            ProcessListener.latency = latency;
            ProcessListener.averageReport = averageReport;
            averageReport = 0;

            // DEBUT BOUCLE
            while (true)
            {

                if (model.processesHasChange)
                {
                    processName = model.getProcessNames();
                }

                int nvCpt = 0;
                String[,] newValues = null;
                if (counter == averageReport)
                {
                    newValues = new String[processName.Count,2];
                }

                Process[] runningNow = Process.GetProcesses();

                foreach (Process process in runningNow.Where(x => processName.Contains(x)))
                {
                    using (PerformanceCounter pcProcess = new PerformanceCounter("Process", "% Processor Time", process.ProcessName))
                    //using (PerformanceCounter memProcess = new PerformanceCounter("Memory", "Available MBytes"))
                    {
                        pcProcess.NextValue();
                       
                        float cpuUseage = pcProcess.NextValue();
                        Console.WriteLine("Process: '{0}' CPU Usage: {1}%", process.ProcessName, cpuUseage);
                        //float memUseage = memProcess.NextValue();
                        //Console.WriteLine("Process: '{0}' RAM Free: {1}MB", process.ProcessName, memUseage);

                        if (counter == averageReport)
                        {
                            newValues[nvCpt,0] = Convert.ToString(processName[nvCpt]);
                            nvCpt++;
                        }

                    }
                }

                averageReport++;

                if (counter == averageReport)
                {
                    model.updateClientValues(newValues, nvCpt);
                    latency = 0;
                }

                Thread.Sleep(ProcessListener.latency);

            }

        }
    }
}