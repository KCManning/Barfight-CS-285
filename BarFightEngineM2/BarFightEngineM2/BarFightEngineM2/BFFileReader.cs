using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

namespace BarFightEngineM2
{
    class BFFileReader
    {
        private string fileAddress;
        private Stream stream;
        private StreamReader reader;
        public BFFileReader(string fileAddress)
        {
            this.fileAddress = fileAddress;
        }

        public void Open()
        {
            stream = TitleContainer.OpenStream(fileAddress);
            reader = new StreamReader(stream);
        }

        public string ReadLine()
        {
            return reader.ReadLine();
        }

        public void Close()
        {
            stream.Close();
            stream.Dispose();
            reader.Dispose();
        }
    }
}
