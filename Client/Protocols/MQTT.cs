using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Sensors;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Subscribing;
using System.Configuration;

namespace Client.Protocols
{
    class MQTT : ProtocolInterface
    {
        IMqttClient mqttClient;
        string baseTopic = ConfigurationManager.AppSettings.Get("company") + "/"+ ConfigurationManager.AppSettings.Get("version") + "/"+ ConfigurationManager.AppSettings.Get("location") + "/";
        private string broker;
        private string clientID = "drone_" + ConfigurationManager.AppSettings.Get("droneID");

        static int numberOfClient = 0;

        public MQTT(string broker)
        {
            this.broker = broker;
            Connect().GetAwaiter().GetResult();
        }

        private async Task<MqttClientConnectResult> Connect()
        {
            var factory = new MqttFactory();

            //update number of client for have different clients id 
            numberOfClient++;

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(this.broker)
                .WithClientId(clientID + numberOfClient)
                .Build();

            mqttClient = factory.CreateMqttClient();

            return await mqttClient.ConnectAsync(options, System.Threading.CancellationToken.None);
        }

        public async void Send(string droneID, List<SensorInterface> sensors)
        {
            foreach (var sensor in sensors)
            {
                //get sensor data
                string data = "{"+sensor.toJson()+"}";
                //get topic - iot2021/*version*/*luogo*/*drone*/status/*sensor*
                string topic = baseTopic + droneID + "/status/" + sensor.getSensorName();

                var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(data)
                .WithExactlyOnceQoS()
                .Build();

                await mqttClient.PublishAsync(message, System.Threading.CancellationToken.None);
            }
        }

        public async void Received(string droneID)
        {
            mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                Console.WriteLine("#RECEIVED COMMAND MESSAGE :");
                //Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                //Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                //Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
                Console.WriteLine();
            });

            // iot2021/*version*/*luogo*/*drone*/command
            string topic = baseTopic + droneID + "/command";

            await mqttClient.SubscribeAsync(topic);
        }
    }
}
