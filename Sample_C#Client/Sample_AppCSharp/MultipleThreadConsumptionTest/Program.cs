using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultipleThreadConsumptionTest
{
    class Program
    {

        public static int nbZipperMax;
        public static int nbZipperLimit;
        public static int nbZipper;
        private static Task listener;
        private static Task[] taskList;

        private static readonly object _sync = new object();


        static void Main(string[] args)
        {
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.BelowNormal;

            nbZipperMax = 5;
            nbZipperLimit = 0;
            nbZipper = 0;

            listener = Task.Factory.StartNew(() => new ApccLib(13000, nbZipperMax).RunThread());

            taskList = new Task[nbZipperMax];
            Random rnd = new Random();

            while(true)
            {
                while (nbZipper < nbZipperLimit)
                {
                    for (int i = 0; i < nbZipperMax; i++)
                    {
                        if (taskList[i] == null)
                        {
                            String rep = @"c:\users\public\reports";
                            int gZipLevel = rnd.Next(1, 10);
                            taskList[i] = Task.Factory.StartNew(() => new GZipper(i, rep, gZipLevel).RunThread());
                            lock (_sync)
                            {
                                nbZipper++;
                            }

                            Console.WriteLine("Nombre de zipper: " + nbZipper + " / " + nbZipperLimit + " / " + nbZipperMax);
                            Thread.Sleep(200);
                        }
                    }
                }

                Thread.Sleep(100);
            }
            
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void zipDoneAndDelete(int id)
        {
            taskList[id] = null;
            lock (_sync)
            {
                nbZipper--;
            }
        }
    }
}
