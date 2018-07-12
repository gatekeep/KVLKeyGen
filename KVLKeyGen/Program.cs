using System;

namespace KVLKeyGen
{
    class Program
    {
        static byte GenerateKeyByte(Random rnd)
        {
            byte ret = 0;
            byte keypart1 = 0, keypart2 = 0;

            // generate random keyparts
            keypart1 = (byte)rnd.Next(0, 15);
            keypart2 = (byte)rnd.Next(0, 15);

            // check keyparts for proper bit-order and parity
            // keypart1 even
            if ((keypart1 == 0x00) || (keypart1 == 0x03) || (keypart1 == 0x05) ||
                (keypart1 == 0x06) || (keypart1 == 0x09) || (keypart1 == 0x0A) ||
                (keypart1 == 0x0C) || (keypart1 == 0x0F))
            {
                if ((keypart2 == 0x00) || (keypart2 == 0x03) || (keypart2 == 0x05) ||
                    (keypart2 == 0x06) || (keypart2 == 0x09) || (keypart2 == 0x0A) ||
                    (keypart2 == 0x0C) || (keypart2 == 0x0F))
                {
                    bool rotateKeypart = true;
                    do
                    {
                        // rotate the keypart to the left
                        if ((keypart2 == 0x00) || (keypart2 == 0x03) || (keypart2 == 0x05) ||
                            (keypart2 == 0x06) || (keypart2 == 0x09) || (keypart2 == 0x0A) ||
                            (keypart2 == 0x0C) || (keypart2 == 0x0F))
                            keypart2++;
                            //keypart2 = (byte)((keypart2 << 1) | (keypart2 >> (4 - 1)));

                        // wrap around to 0 if we exceed 15
                        if (keypart2 > 0x0F)
                            keypart2 = 0x00;

                        if ((keypart2 == 0x01) || (keypart2 == 0x02) || (keypart2 == 0x04) ||
                            (keypart2 == 0x07) || (keypart2 == 0x08) || (keypart2 == 0x0B) ||
                            (keypart2 == 0x0D) || (keypart2 == 0x0E))
                            rotateKeypart = false;
                    } while (rotateKeypart);
                }
            }

            // keypart1 odd
            if ((keypart1 == 0x01) || (keypart1 == 0x02) || (keypart1 == 0x04) ||
                (keypart1 == 0x07) || (keypart1 == 0x08) || (keypart1 == 0x0B) ||
                (keypart1 == 0x0D) || (keypart1 == 0x0E))
            {
                if ((keypart2 == 0x01) || (keypart2 == 0x02) || (keypart2 == 0x04) ||
                    (keypart2 == 0x07) || (keypart2 == 0x08) || (keypart2 == 0x0B) ||
                    (keypart2 == 0x0D) || (keypart2 == 0x0E))
                {
                    bool rotateKeypart = true;
                    do
                    {
                        // rotate the keypart to the left
                        if ((keypart2 == 0x01) || (keypart2 == 0x02) || (keypart2 == 0x04) ||
                            (keypart2 == 0x07) || (keypart2 == 0x08) || (keypart2 == 0x0B) ||
                            (keypart2 == 0x0D) || (keypart2 == 0x0E))
                            keypart2++;
                            //keypart2 = (byte)((keypart2 >> 1) | (keypart2 << (4 - 1)));

                        // wrap around to 0 if we exceed 15
                        if (keypart2 > 0x0F)
                            keypart2 = 0x00;

                        if ((keypart2 == 0x00) || (keypart2 == 0x03) || (keypart2 == 0x05) ||
                            (keypart2 == 0x06) || (keypart2 == 0x09) || (keypart2 == 0x0A) ||
                            (keypart2 == 0x0C) || (keypart2 == 0x0F))
                            rotateKeypart = false;
                    } while (rotateKeypart);
                }
            }

            // OR resulting bytes together
            ret = (byte)(keypart1 << 4);
            ret |= (byte)(keypart2);

            return ret;
        }

        static ushort GenerateKeyUShort(Random rnd)
        {
            ushort ret = 0;
            byte keypart1 = 0, keypart2 = 0;

            keypart1 = GenerateKeyByte(rnd);
            keypart2 = GenerateKeyByte(rnd);

            // OR resulting bytes together
            ret = (ushort)(keypart1 << 8);
            ret |= (ushort)(keypart2);

            return ret;
        }

        static long CountBits(ulong value)
        {
            long count = 0;
            while (value != 0)
            {
                count++;
                value &= value - 1;
            }
            return count;
        }

        static void Main(string[] args)
        {
            Random rnd = new Random();

            const ushort _7070 = 0x7070;
            const ushort _8F8F = 0x8F8F;
            const ushort _AD6D = 0xAD6D;

            Console.WriteLine("Test SLNs:");
            Console.Write(string.Format("SLN {0} Key ", "T1"));
            for (int j = 0; j < 4; j++)
            {
                ushort keypart = _7070;
                Console.Write(string.Format("{0} ({1}) ", keypart.ToString("X4"), CountBits(keypart).ToString("D2")));
            }
            Console.WriteLine();

            Console.Write(string.Format("SLN {0} Key ", "T2"));
            for (int j = 0; j < 4; j++)
            {
                ushort keypart = _8F8F;
                Console.Write(string.Format("{0} ({1}) ", keypart.ToString("X4"), CountBits(keypart).ToString("D2")));
            }
            Console.WriteLine();

            Console.Write(string.Format("SLN {0} Key ", "T3"));
            for (int j = 0; j < 4; j++)
            {
                ushort keypart = _AD6D;
                Console.Write(string.Format("{0} ({1}) ", keypart.ToString("X4"), CountBits(keypart).ToString("D2")));
            }
            Console.WriteLine();

            Console.WriteLine("\n16 randomly generated SLNs:");
            for (int i = 0; i < 16; i++)
            {
                Console.Write(string.Format("SLN {0} Key ", i.ToString("D2")));
                for (int j = 0; j < 4; j++)
                {
                    ushort keypart = GenerateKeyUShort(rnd);
                    Console.Write(string.Format("{0} ({1}) ", keypart.ToString("X4"), CountBits(keypart).ToString("D2")));
                }
                Console.WriteLine();
            }
        }
    }
}
