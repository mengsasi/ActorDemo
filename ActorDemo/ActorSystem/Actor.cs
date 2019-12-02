using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorDemo.ActorSystem
{

    public interface IActor
    {
        string Name { get; set; }

        bool Exited { get; }//是否退出

        int MessageCount { get; }

        ActorContext Context { get; }

        void Enqueue(MsgData data);

        void Dequeue();

    }

    public abstract class Actor : IActor
    {

        protected Actor()
        {
            context = new ActorContext(this);
        }

        string IActor.Name { get; set; }

        private bool exited = false;
        bool IActor.Exited {
            get {
                return exited;
            }
        }

        int IActor.MessageCount {
            get {
                return messageQueue.Count;
            }
        }

        private ActorContext context;
        ActorContext IActor.Context {
            get {
                return context;
            }
        }

        private Queue<MsgData> messageQueue = new Queue<MsgData>();

        protected abstract void Receive(MsgData message);

        void IActor.Enqueue(MsgData message)
        {
            if (exited)
                return;
            lock (messageQueue)
            {
                messageQueue.Enqueue(message);
            }
        }

        void IActor.Dequeue()
        {
            MsgData message;
            lock (messageQueue)
            {
                message = messageQueue.Dequeue();
            }
            Receive(message);
        }

        protected void Exit()
        {
            exited = true;
        }
    }

}
