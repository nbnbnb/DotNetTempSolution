using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace MyDebug
{
    class Program
    {
        static void Main(string[] args)
        {
            int a = 1;
            int b = 2;
            int c = a + b;
            int d = 0;
            int e = c / d;
            Console.WriteLine(e);

            SortedList<int, int> gg = null;
        }
    }



}
