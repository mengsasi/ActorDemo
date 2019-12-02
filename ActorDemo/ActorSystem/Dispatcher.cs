using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActorDemo.ActorSystem
{

    public class Dispatcher
    {

        public static Dispatcher instance;
        public static Dispatcher Instance {
            get { return instance ?? (instance = new Dispatcher()); }
        }

        public void ReadyToExecute(IActor actor)
        {
            if (actor.Exited)
                return;

            //Status和WAITING比较，相等就用EXECUTING替换Status
            int status = Interlocked.CompareExchange(ref actor.Context.Status, ActorContext.EXECUTING, ActorContext.WAITING);

            if (status == ActorContext.WAITING)
            {
                //将方法排入队列以便执行，并指定包含该方法所用数据的对象。 此方法在有线程池线程变得可用时执行
                ThreadPool.QueueUserWorkItem(Execute, actor);
            }
        }

        private void Execute(object o)
        {
            IActor actor = (IActor)o;
            actor.Execute();

            if (actor.Exited)
            {
                Thread.VolatileWrite(ref actor.Context.Status, ActorContext.EXITED);
            }
            else
            {
                Thread.VolatileWrite(ref actor.Context.Status, ActorContext.WAITING);
                if (actor.MessageCount > 0)
                {
                    ReadyToExecute(actor);
                }

            }
        }

    }
}
