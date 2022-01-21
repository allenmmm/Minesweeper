using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Minesweeper
{
    public static class Utils
    {



        //tbd write static tests for checking range
 
        public static int GetRandomInt(int max, Random random) 
        {
            var rnd = new Random(10);

            Thread.Sleep(20);
           // var rnd = new Random();
            return rnd.Next(max);
        }
    }
}
