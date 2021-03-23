using System;
using System.Buffers.Binary;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Steganography
{
    public class Utils
    {
        // This Quantization table is used to determine the number of bits to use to hide a message in a pixel.
        static public Dictionary<int[] , int> QuantizationTable = new Dictionary<int[], int>()
        {
            { new int[] {0, 1}, 1},
            { new int[] {2, 32}, 2},
            { new int[] {33, 64}, 3},
            { new int[] {65, 255}, 4}
        };
        /*
         public static int[] TextToBin(string secret)
        {
            int[] res = new int[secret.Length * 8];
            int k = 7;
            foreach (bool v in new BitArray(Encoding.ASCII.GetBytes(secret)))
            {
                if (k == -1)
                    k += 16;
                res[k] = v ? 1 : 0;
                k--;
            }
            return res;
        }
         */
        // This function converts a string into an array of bits.
        public static int[] TextToBin(string secret)
        {
            int[] res = new int[secret.Length * 8];
            int k = 0;
            foreach (char v in secret)
            {
                int com = 128;
                for (int i = 0; i < 8; i++)
                {
                    res[k] = (v & com)==0 ? 0 : 1;
                    com >>= 1;
                    k++;
                }
                
            }
            return res;
        }
        
        // This function converts an array of bits into a string.
        // We consider that the array length is a multiple of 8.
        /*
         public static string BinToText(int[] bin)
        {
            int lenBin = bin.Length;
            byte[] res = new byte[lenBin/8];
            int ii = 0;
            for (int i = 0; i < lenBin; i+=8)
            {
                bool[] tab = new bool[8];
                for (int j = 7; j >= 0; j--)
                    tab[7-j] = bin[i + j] == 1;
                (new BitArray(tab)).CopyTo(res,ii);
                ii++;
            }
            return new String(Encoding.ASCII.GetChars(res));
        }
         */
        public static string BinToText(int[] bin)
        {
            int lenBin = bin.Length;
            string res = "";
            for (int i = 0; i < lenBin; i+=8)
            {
                int v = 0;
                for (int j = 0; j < 8; j++)
                {
                    v <<= 1;
                    v += bin[i + j];
                }
                res += (char) v;
            }
            return res;
        }

        // This function extract the nbBits bits beginning from the index position from the secret array.
        // It is also converting the binary result into decimal.
        public static int ExtractBits(int[] secret, int index, int nbBits)
        {
            int res = 0;
            int lenSecret = secret.Length;
            int max = nbBits + index;
            for (int i = index; i < max; i++)
            {
                res <<= 1;
                res += i >= lenSecret ? 0 : secret[i];
            }
            return res;
        }

        // This function translates the int value (in decimal) into binary in the secret array.
        public static void InsertBits(int[] secret, int index, int nbBits, int value)
        {
            int lenSecret = secret.Length;
            int max = nbBits;
            if (max + index > lenSecret)
                max -= max + index - lenSecret;
            int del = 1 << nbBits;
            for (int i = 0; i < max; i++)
            {
                del >>= 1;
                secret[i + index] = (value & del) == 0 ? 0 : 1;
            }
        }

        // Assuming we already have grey pixels (R = G = B).
        // This is | pixel1.R - pixel2.R |.
        public static int GetDifference(Color pixel1, Color pixel2)
        {
            return pixel1.R < pixel2.R ? pixel2.R - pixel1.R : pixel1.R - pixel2.R;
        }

        
        // This function opens an image.
        public static Bitmap OpenImage(string path)
        {
            return new Bitmap(path);
        }

        // This function saves the image into the file 'name'.
        public static void SaveImage(string name, Bitmap image)
        {
            image.Save(name);
            image.Dispose();
        }
        
        // This function clears the nbBits LSB of the int color.
        public static int ClearLSB(int color, int nbBits)
        {
            if (color <= 0)
                return color;
            color >>= nbBits;
            color <<= nbBits;
            return color;
        }
        
        // This function replaces the nbBits LSB by newLSB.
        public static int ReplaceLSB(int color, int nbBits, int newLSB)
        {
            color = ClearLSB(color, nbBits);

            return color + newLSB;
        }
        
        // This function saves ONLY the nbBits LSB of the int color.
        public static int SaveLSB(int color, int nbBits)
        {
            if (color <= 0)
                return color;
            color <<= 8 - nbBits;
            color %= 256;
            color >>= 8 - nbBits;
            return color;
        }
    }
}