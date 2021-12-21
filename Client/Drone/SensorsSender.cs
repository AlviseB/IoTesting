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
                String json = getJsonString((string)droneID, sensors);
                try
                {
                    protocol.Send(json);
                    Console.WriteLine("Data sent: " + json);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Sending error: " + ex);
                }
                System.Threading.Thread.Sleep(1000);
            }
        }

        static string getJsonString(String droneID, List<SensorInterface> sensors)
        {
            String json = "{";
            //add drone ID in json string
            json += "\"ID\": \"" + droneID + "\"";
            //add sensors in json string
            foreach (SensorInterface sensor in sensors)
            {
                json += ", ";
                string json_sensor = sensor.toJson();
                json += json_sensor;
            }
            json += '}';

            return json;
        }
    }
}
