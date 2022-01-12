using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Sensors;
using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;

namespace Client.Protocols
{
    class MQTT : ProtocolInterface
    {
        IManagedMqttClient mqttClient;
        string basePath = "iot2021/thiene/";
        public MQTT(string server, int port)
        {
            // Creates a new client
            MqttClientOptionsBuilder builder = new MqttClientOptionsBuilder()
                                                    .WithClientId("3e63e4a657af4f7d8a7454fe1e3cd269")
                                                    .WithTcpServer(server, port);

            // Create client options objects
            ManagedMqttClientOptions options = new ManagedMqttClientOptionsBuilder()
                                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(60))
                                    .WithClientOptions(builder.Build())
                                    .Build();

            // Creates the client object
            mqttClient = new MqttFactory().CreateManagedMqttClient();

            // Set up handlers
            mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnConnected);
            mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnDisconnected);
            mqttClient.ConnectingFailedHandler = new ConnectingFailedHandlerDelegate(OnConnectingFailed);

            // Starts a connection with the Broker
            mqttClient.StartAsync(options).GetAwaiter().GetResult();
        }

        public static void OnConnected(MqttClientConnectedEventArgs obj)
        {
            Console.WriteLine("Successfully connected.");
        }

        public static void OnConnectingFailed(ManagedProcessFailedEventArgs obj)
        {
            Console.WriteLine("Couldn't connect to broker.");
        }

        public static void OnDisconnected(MqttClientDisconnectedEventArgs obj)
        {
            Console.WriteLine("Successfully disconnected.");
        }

        public void Send(string droneID, List<SensorInterface> sensors)
        {
            foreach (var sensor in sensors)
            {
                //get sensor data
                string data = "{"+sensor.toJson()+"}";
                //get path ("iot2021/thiene/"+ drone id + sensor name)
                string path = basePath + droneID + "/" + sensor.getSensorName();
                mqttClient.PublishAsync(path, data);
            }
        }

        public string Received(string droneID)
        {
            return "TEST";
        }
    }
}
