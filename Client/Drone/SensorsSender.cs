using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Sensors;
using Client.Protocols;

namespace Client.Drone
{
    class SensorsSender
    {
        static public void doWork(object parameters)
        {
            //extract id and protocols from dictionary
            Dictionary<string, object> dict = (Dictionary<string, object>)parameters;
            string droneID = (string) dict["droneID"];
            ProtocolInterface protocol = (ProtocolInterface) dict["protocol"];

            // init sensors
            List<SensorInterface> sensors = new List<SensorInterface>
            {
                new Timestamp(),
                new VirtualSpeedSensor(),
                new VirtualGPSSensor(),
                new VirtualAltitudeSensor(),
                new VirtualOrientation(),
                new VirtualBattery()
            };

            // send data to server
            while (true)
            {
                try
                {
                    protocol.Send(droneID, sensors);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Sending error: " + ex);
                }
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
