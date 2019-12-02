using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActorDemo.ActorSystem;

namespace ActorDemo
{

    public class Counter : Actor
    {

        private string m_value;

        public Counter() { }

        protected override void Receive(MsgData message)
        {
            string msg = message.UnPack();

            m_value += msg + "\n";

            if (msg == "stop")
            {
                Console.WriteLine(m_value);
                Exit();
            }

        }
    }
}
