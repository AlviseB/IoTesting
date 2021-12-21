using System;
using System.Collections.Generic;
using Client.Sensors;
using Client.Protocols;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {

            // init sensors
            List<SensorInterface> sensors = new List<SensorInterface>();

            sensors.Add(new VirtualSpeedSensor());
            sensors.Add(new VirtualGPSSensor());
            sensors.Add(new VirtualAltitudeSensor());

            // define protocol
            ProtocolInterface protocol = new Http("http://10.30.134.34:8011/drones/123");

            // send data to server
            while (true)
            {
                String json = "{";
                bool firstLoop = true;
                foreach (SensorInterface sensor in sensors)
                {
                    if (!firstLoop)
                        json += ", ";
                    firstLoop = false;
                    string json_sensor = sensor.toJson();
                    json += json_sensor;
                }
                json += '}';
                //protocol.Send(json);
                Console.WriteLine("Data sent: " + json);
                System.Threading.Thread.Sleep(1000);
            }

        }

    }

}
