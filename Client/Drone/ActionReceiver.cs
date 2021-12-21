using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
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

            while (true)
            {
                try
                {
                    string json = protocol.Received(droneID);
                    Dictionary<string, string> action = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    Console.WriteLine(action["command"]);
                }catch(Exception ex)
                {
                    Console.WriteLine("Receiving error: " + ex);
                }
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
