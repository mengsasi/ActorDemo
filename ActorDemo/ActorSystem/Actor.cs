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

        void Execute();//执行

    }

    public abstract class Actor<T> : IActor
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

        private Queue<T> messageQueue = new Queue<T>();

        protected abstract void Receive(T message);

        public void Send(T message, string targetName)
        {
            if (exited)
                return;
            Dispatcher.Instance.ReadyToExecute(targetName);
        }

        public void Enqueue(T message)
        {
            if (exited)
                return;
            lock (messageQueue)
            {
                messageQueue.Enqueue(message);
            }
        }

        void IActor.Execute()
        {
            T message;
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
