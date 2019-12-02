using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorDemo.ActorSystem {

    public class MsgData {

        public MsgData() {
        }

        public MsgData( string msg ) {
            Pack( msg );
        }

        private string message;

        public void Pack( string msg ) {
            message = msg;
        }

        public string UnPack() {
            return message;
        }

    }
}
