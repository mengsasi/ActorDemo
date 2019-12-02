using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActorDemo.ActorSystem;

namespace ActorDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            Counter counter1 = new Counter();
            Counter counter2 = new Counter();

            Dispatcher.Instance.Register("counter1", counter1);
            Dispatcher.Instance.Register("counter2", counter2);

            for (int i = 0; i < 10000; i++)
            {
                counter1.Send(i, "counter2");

                counter2.Send(10, "counter1");
            }

            counter1.Send(-1, "counter2");

            counter2.Send(-1, "counter2");

            Console.ReadLine();

        }
    }
}
