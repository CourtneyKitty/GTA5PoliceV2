using System;
using System.Collections.Generic;
using System.Text;

namespace GTA5PoliceV2.Util
{
    public class Array
    {
        public static byte[] Insert(byte[] array, byte item)
        {
            List<byte> list = new List<byte>();
            for (int i = 0; i < array.Length; i++)
            {
                list.Add(array[i]);
            }
            list.Add(item);
            array = list.ToArray();
            return array;
        }

        public static int[] Insert(int[] array, int item)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < array.Length; i++)
            {
                list.Add(array[i]);
            }
            list.Add(item);
            array = list.ToArray();
            return array;
        }

        public static double[] Insert(double[] array, double item)
        {
            List<double> list = new List<double>();
            for (int i = 0; i < array.Length; i++)
            {
                list.Add(array[i]);
            }
            list.Add(item);
            array = list.ToArray();
            return array;
        }

        public static ulong[] Insert(ulong[] array, ulong item)
        {
            List<ulong> list = new List<ulong>();
            for (int i = 0; i < array.Length; i++)
            {
                list.Add(array[i]);
            }
            list.Add(item);
            array = list.ToArray();
            return array;
        }

        public static string[] Insert(string[] array, string item)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < array.Length; i++)
            {
                list.Add(array[i]);
            }
            list.Add(item);
            array = list.ToArray();
            return array;
        }
    }
}
