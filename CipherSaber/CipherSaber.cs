using Arcfour;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CipherSaber
{
    class CipherSaber
    {
        static void Main(string[] args)
        {
            //Stopwatch stopWatch = new Stopwatch();
            //Console.WriteLine("Warming up...");
            //Thread.Sleep(2000);
            //Console.WriteLine("Go.\n");

            //byte b = 0;
            //int size = 100 * 1024 * 1024;
            //for (int k = 0; k < 5; k++)
            //{
            //    RC4 rc4 = new RC4("boomstick");
            //    stopWatch.Restart();
            //    for (int i = 0; i < size; i++)
            //        b = rc4.GetNextByte();

            //    stopWatch.Stop();
            //    Console.WriteLine(string.Format("{2} MB Stream time: {0:n3} sec\nLast byte: 0x{1:x}\n", stopWatch.Elapsed.TotalSeconds, b, size / 1024 / 1024));
            //}

            //Console.ReadKey();

            RC4 rc4 = new RC4("boomstick");
            string orig = "Hello World";
            string enc = rc4.CryptString(orig);
            rc4 = new RC4("boomstick");
            string dec = rc4.CryptString(enc);
            Console.WriteLine(string.Format("Original: {0}\nEncrypted: {1}\nDecrypted: {2}", orig, enc, dec));

            Console.ReadKey();

            //string orig = @"C:\Users\epilsits\Desktop\CSU2.1_For_RDI_Feb2017.msi.zip";
            //string enc = @"C:\Users\epilsits\Desktop\enc.dat";
            //string dec = @"C:\Users\epilsits\Desktop\orig.zip";

            //RC4 rc4 = new RC4("boomstick");
            //rc4.CryptFile(orig, enc);

            //rc4 = new RC4("boomstick");
            //rc4.CryptFile(enc, dec);
        }
    }
}
