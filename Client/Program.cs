using System;
using System.Collections.Generic;
using Client.Sensors;
using Client.Protocols;
using Client.Drone;
using System.Threading;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            String droneID = "dr1_42";

            //thread for send sensors data
            Thread senderThread = new Thread(SensorsSender.doWork);
            senderThread.Start(droneID);

        }

    }

}
