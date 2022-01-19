using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Protocols;

namespace Client.Drone
{
    class ActionReceiver
    {
        static public void doWork(object parameters)
        {
            //extract id and protocols from dictionary
            Dictionary<string, object> dict = (Dictionary<string, object>)parameters;
            string droneID = (string)dict["droneID"];
            ProtocolInterface protocol = (ProtocolInterface)dict["protocol"];

            //call received protocol method
            protocol.Received(droneID);
        }
    }
}
