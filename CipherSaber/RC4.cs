using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcfour
{
    class RC4
    {
        private byte[] Key { get; set; }
        private IEnumerator<byte> Gen { get; }

        public RC4(byte[] key, int rounds = 1)
        {
            if (key.Length > 256)
                throw new ArgumentException("Max key length exceeded.");

            InitKey(key, rounds);
            Gen = GenBytes().GetEnumerator();
        }

        public RC4(string keytext, int rounds = 1, string encoding = "iso-8859-1")
            : this(Encoding.GetEncoding(encoding).GetBytes(keytext), rounds) // also Latin1; 8-bit character set
        {           
        }

        private void InitKey(byte[] key, int rounds)
        {
            int kl = key.Length;
            Key = Enumerable.Range(0, 256).Select(i => Convert.ToByte(i)).ToArray();
            int j = 0;
            for (int r = 0; r < rounds; r++)
                for (int i = 0; i < 256; i++)
                {
                    j = (j + Key[i] + key[i % kl]) % 256;
                    Swap(ref Key[i], ref Key[j]);
                }
        }

        private IEnumerable<byte> GenBytes()
        {
            int i = 0;
            int j = 0;
            while (true)
            {
                i = ++i % 256;
                j = (j + Key[i]) % 256;
                Swap(ref Key[i], ref Key[j]);
                yield return Key[(Key[i] + Key[j]) % 256];
            }
        }

        public byte GetNextByte()
        {
            Gen.MoveNext();
            return Gen.Current;
        }

        public byte[] CryptBytes(byte[] bytes, int len)
        {
            for (int i = 0; i < len; i++)
                bytes[i] = (byte)(bytes[i] ^ GetNextByte());

            return bytes;
        }

        public byte[] CryptBytes(byte[] bytes)
        {
            return CryptBytes(bytes, bytes.Length);
        }

        public string CryptString(string str, string encoding = "iso-8859-1")
        {
            var enc = Encoding.GetEncoding(encoding);
            return enc.GetString(CryptBytes(enc.GetBytes(str)));
        }

        public void CryptFile(string inFile, string outFile)
        {
            if (!File.Exists(inFile))
                throw new ArgumentException("Input file does not exist.");

            byte[] buff = new byte[1048576];
            using (var input = File.OpenRead(inFile))
            using (var output = File.OpenWrite(outFile))
            {
                int read;
                while ((read = input.Read(buff, 0, 1048576)) > 0)
                    output.Write(CryptBytes(buff, read), 0, read);
            }
        }

        #region Utility
        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp = lhs;
            lhs = rhs;
            rhs = temp;
        }
        #endregion
    }
}
