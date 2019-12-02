using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActorDemo.ActorSystem
{

    //两种方式
    //一种，类似Unity的Update，一直执行发送，接收事件的方法，内部处理
    //一种，就是现在这个，我发消息，对方才执行处理方法，处理完所有消息，就不占用线程资源

    public class Dispatcher
    {

        private static Dispatcher instance;
        public static Dispatcher Instance {
            get { return instance ?? (instance = new Dispatcher()); }
        }

        private Dictionary<string, IActor> actorDict = new Dictionary<string, IActor>();

        public bool Register(string name, IActor actor)
        {
            IActor ac;
            if (actorDict.TryGetValue(name, out ac))
            {
                //已存在
                return false;
            }
            actor.Name = name;
            actorDict[name] = actor;
            return true;
        }

        public void UnRegister(string name)
        {
            if (actorDict.ContainsKey(name))
            {
                actorDict.Remove(name);
            }
        }

        //skynet send call
        //发消息写到了外面，从Actor里拿出来了
        public void Send(MsgData message, string actorName)
        {
            IActor actor;
            if (actorDict.TryGetValue(actorName, out actor))
            {
                if (actor.Exited)
                    return;
                actor.Enqueue(message);

                ReadyToExecute(actor);
            }
        }

        private void ReadyToExecute(IActor actor)
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
            actor.Dequeue();
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
