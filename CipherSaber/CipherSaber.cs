using Arcfour;
using CS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CS
{
    static class CipherSaber
    {
        const int buffsize = 1048576;

        public static void EncryptFile(string infile, string outfile, string key, int rounds = 20, string encoding = "iso-8859-1")
        {
            if (!File.Exists(infile))
                throw new ArgumentException("Input file does not exist.");

            byte[] buff = new byte[buffsize];
            byte[] iv = new byte[10];
            int read;

            using (var input = File.OpenRead(infile))
            using (var output = File.OpenWrite(outfile))
            {
                var rand = new Random();
                rand.NextBytes(iv);

                output.Write(iv, 0, iv.Length);

                List<byte> keybytes = new List<byte>();
                keybytes.AddRange(Encoding.GetEncoding(encoding).GetBytes(key.Substring(0, Math.Min(key.Length, 246))));
                keybytes.AddRange(iv);

                var rc4 = new RC4(keybytes.ToArray(), rounds);
                while ((read = input.Read(buff, 0, buffsize)) > 0)
                    output.Write(rc4.CryptBytes(buff, read), 0, read);
            }
        }

        public static void DecryptFile(string infile, string outfile, string key, int rounds = 20, string encoding = "iso-8859-1")
        {
            if (!File.Exists(infile))
                throw new ArgumentException("Input file does not exist.");

            byte[] buff = new byte[buffsize];
            byte[] iv = new byte[10];
            int read;

            using (var input = File.OpenRead(infile))
            {
                if ((read = input.Read(iv, 0, 10)) != 10)
                    throw new Exception("Invalid input file.");

                using (var output = File.OpenWrite(outfile))
                {
                    List<byte> keybytes = new List<byte>();
                    keybytes.AddRange(Encoding.GetEncoding(encoding).GetBytes(key.Substring(0, Math.Min(key.Length, 246))));
                    keybytes.AddRange(iv);

                    var rc4 = new RC4(keybytes.ToArray(), rounds);
                    while ((read = input.Read(buff, 0, buffsize)) > 0)
                        output.Write(rc4.CryptBytes(buff, read), 0, read);
                }
            }
        }
    }
}

namespace MyCS
{
    class Program
    {
        static void Main(string[] args)
        {
            string forig = @"C:\Users\epilsits\Desktop\cknight.cs1";
            string fin = @"C:\Users\epilsits\Desktop\cknight.gif";
            string fout = @"C:\Users\epilsits\Desktop\cknight2.gif";
            string fnew = @"C:\Users\epilsits\Desktop\new.cs1";

            CipherSaber.DecryptFile(forig, fin, "ThomasJefferson", 1);
            //CipherSaber.EncryptFile(fin, fnew, "ThomasJefferson", 1);
            //CipherSaber.DecryptFile(fnew, fout, "ThomasJefferson", 1);

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

            //RC4 rc4 = new RC4("boomstick");
            //string orig = "Hello World";
            //string enc = rc4.CryptString(orig);
            //rc4 = new RC4("boomstick");
            //string dec = rc4.CryptString(enc);
            //Console.WriteLine(string.Format("Original: {0}\nEncrypted: {1}\nDecrypted: {2}", orig, enc, dec));

            //Console.ReadKey();

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
