using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            Counter counter = new Counter();
            for (int i = 0; i < 10000; i++)
            {
                counter.Post(i);
            }

            counter.Post(-1);

            Console.ReadLine();

        }
    }
}
