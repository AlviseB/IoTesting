using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Protocols
{
    interface ProtocolInterface
    {
        void Send(string droneID, Dictionary<string, string> sensors);
        void Received(string droneID);
    }
}
