using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Sensors;

namespace Client.Protocols
{
    interface ProtocolInterface
    {
        void Send(string droneID, List<SensorInterface> sensors);
        void Received(string droneID);
    }
}
