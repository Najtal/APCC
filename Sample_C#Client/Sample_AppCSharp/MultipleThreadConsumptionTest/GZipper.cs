using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultipleThreadConsumptionTest
{
    class GZipper //: BaseThread
    {
        private int id;
        private string rep;
        private int gZipLevel;
        private string filePath;
        private static int counter = 0;

        private static readonly object _sync = new object();

        public GZipper(int id, string rep, int gZipLevel)
        {
            this.id = id;
            this.rep = rep;
            this.gZipLevel = gZipLevel;
        }

        public void RunThread()
        {

            string directoryPath = rep;
            DirectoryInfo directorySelected = new DirectoryInfo(directoryPath);
            FileInfo fileToCompress = directorySelected.EnumerateFiles().First();

            using (FileStream originalFileStream = fileToCompress.OpenRead())
            {
                if ((File.GetAttributes(fileToCompress.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
                {
                    lock (_sync)
                    {
                        counter++;
                    }
                    
                    using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + "_" + id + ".gz"))
                    {
                        using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionLevel.Optimal, true))
                        {
                            originalFileStream.CopyTo(compressionStream);
                        }
                    }
                }
            }

            Program.zipDoneAndDelete(id);
        }
    }
}
