using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActorDemo.ActorSystem;

namespace ActorDemo
{

    public class Counter : Actor<int>
    {

        private int m_value;

        public Counter() : this(0) { }

        public Counter(int initial)
        {
            m_value = initial;
        }

        protected override void Receive(int message)
        {
            m_value += message;

            if (message == -1)
            {
                Console.WriteLine(m_value);
                Exit();
            }
        }
    }
}
