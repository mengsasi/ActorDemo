using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorDemo.ActorSystem {

    public class ActorContext {

        public ActorContext( IActor actor ) {
            Actor = actor;
        }

        public IActor Actor { get; private set; }

        public const int WAITING = 0;//等待
        public const int EXECUTING = 1;//执行
        public const int EXITED = 2;//退出

        public int Status;

    }

}
