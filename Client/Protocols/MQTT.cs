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

namespace Client.Protocols
{
    class MQTT : ProtocolInterface
    {
        IMqttClient mqttClient;
        string baseTopic = "iot2021/thiene/";
        private string broker;

        public MQTT(string broker)
        {
            this.broker = broker;
            Connect().GetAwaiter().GetResult();
        }

        private async Task<MqttClientConnectResult> Connect()
        {
            var factory = new MqttFactory();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(this.broker)
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
                //get topic ("iot2021/thiene/"+ drone id + sensor name)
                string topic = baseTopic + droneID + "/" + sensor.getSensorName();

                var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(data)
                .WithExactlyOnceQoS()
                .Build();

                await mqttClient.PublishAsync(message, System.Threading.CancellationToken.None);
            }
        }

        public void Received(string droneID)
        {
            string topic = baseTopic + droneID + "/#";
            Console.WriteLine(topic);
            
            mqttClient.UseConnectedHandler(async e =>
            {
                // Subscribe to a topic
                await mqttClient.SubscribeAsync("iot2021/#");
            });

            mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
                Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
                Console.WriteLine();

                Task.Run(() => mqttClient.PublishAsync("hello/world"));
            });
        }
    }
}
