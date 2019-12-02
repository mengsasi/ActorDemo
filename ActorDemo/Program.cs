using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActorDemo.ActorSystem;

namespace ActorDemo {

    class Program {

        static void Main( string[] args ) {

            Counter counter1 = new Counter();
            Counter counter2 = new Counter();

            Dispatcher.Instance.Register( "counter1", counter1 );
            Dispatcher.Instance.Register( "counter2", counter2 );

            for( int i = 0; i < 10000; i++ ) {
                MsgData data1 = new MsgData( i.ToString() );

                Dispatcher.Instance.Send( data1, "counter2" );
            }

            for( int i = 0; i < 10000; i++ ) {
                MsgData data2 = new MsgData( "=========" );

                Dispatcher.Instance.Send( data2, "counter1" );
            }


            MsgData data = new MsgData( "stop" );

            Dispatcher.Instance.Send( data, "counter1" );

            Dispatcher.Instance.Send( data, "counter2" );

            Console.ReadLine();

        }
    }
}
