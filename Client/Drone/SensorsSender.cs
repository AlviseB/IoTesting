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

            List<SensorInterface> sensors = (List<SensorInterface>) dict["sensors"];

            // read and send data
            while (true)
            {
                Dictionary<string, string> sensors_data = new Dictionary<string, string>();

                //read data from sensors
                foreach(SensorInterface sensor in sensors) {
                    //read value as a string -> 'sensorname': value
                    sensors_data[sensor.getSensorName()] = sensor.toJson();
                }


                try
                {
                    //send data with protocol
                    protocol.Send(droneID, sensors_data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Sending error: " + ex);
                }
                //sleep 1 second
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
