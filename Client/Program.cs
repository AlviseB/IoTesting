using System;
using System.Collections.Generic;
using Client.Sensors;
using Client.Protocols;
using Client.Drone;
using System.Threading;
using System.Configuration;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            String droneID = ConfigurationManager.AppSettings.Get("droneID");

            // define HTTP protocol
            //ProtocolInterface sender_protocol = new Http("http://10.30.134.34:8011");
            //ProtocolInterface receiver_protocol = new Http("http://10.30.134.34:8011");

            // define MQTT protocol
            //ProtocolInterface sender_protocol = new MQTT("test.mosquitto.org");
            //ProtocolInterface receiver_protocol = new MQTT("test.mosquitto.org");

            // define CoAP protocol
            //ProtocolInterface sender_protocol = new CoAP("localhost");
            //ProtocolInterface receiver_protocol = new CoAP("localhost");

            // define AMQP protocol
            ProtocolInterface sender_protocol = new AMQP("amqps://kxbxxcqx:IqK-VIirhbu-Gmil2bED0EsPXUxfeaa2@goose.rmq2.cloudamqp.com/kxbxxcqx");
            ProtocolInterface receiver_protocol = new AMQP("amqps://kxbxxcqx:IqK-VIirhbu-Gmil2bED0EsPXUxfeaa2@goose.rmq2.cloudamqp.com/kxbxxcqx");

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

            //create thread parameter
            Dictionary<string, object> parameter = new Dictionary<string, object>
            {
                ["droneID"] = droneID,
            };

            parameter["protocol"] = sender_protocol;
            parameter["sensors"] = sensors;

            //thread for send sensors data
            Thread senderThread = new Thread(SensorsSender.doWork);
            senderThread.Start(parameter);

            Thread.Sleep(100);

            parameter["protocol"] = receiver_protocol;
            //thread for receive action data
            Thread receiverThread = new Thread(ActionReceiver.doWork);
            receiverThread.Start(parameter);
        }

    }

}
