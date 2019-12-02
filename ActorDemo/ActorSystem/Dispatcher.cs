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

        public void PrepareExecute<T>(T msg, string actorName)
        {
            IActor actor;
            if (actorDict.TryGetValue(actorName, out actor))
            {
                if (actor.Exited)
                    return;

                ReadyToExecute(actorName);
            }
        }

        public void ReadyToExecute(string actorName)
        {
            IActor actor;
            if (actorDict.TryGetValue(actorName, out actor))
            {
                if (actor.Exited)
                    return;
                //Status和WAITING比较，相等就用EXECUTING替换Status
                int status = Interlocked.CompareExchange(ref actor.Context.Status, ActorContext.EXECUTING, ActorContext.WAITING);

                if (status == ActorContext.WAITING)
                {
                    //将方法排入队列以便执行，并指定包含该方法所用数据的对象。 此方法在有线程池线程变得可用时执行
                    ThreadPool.QueueUserWorkItem(Execute, actorName);
                }
            }
        }

        private void Execute(object o)
        {
            string actorName = (string)o;
            IActor actor;
            if (actorDict.TryGetValue(actorName, out actor))
            {
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
                        ReadyToExecute(actorName);
                    }

                }
            }
        }

    }
}
