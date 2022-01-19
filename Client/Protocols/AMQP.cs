using Client.JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Client.Protocols
{
    class AMQP : ProtocolInterface
    {
        private string URI;
   
        public AMQP (string URI)
        {
            this.URI = URI;
        }

        public void Send(string droneID, Dictionary<string, string> sensors)
        {
            //assemble sensors data in a single json string
            string data = JsonManager.assembleJSON(droneID, sensors);

            //open connection
            var factory = new ConnectionFactory() { Uri = new Uri(URI) };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            
            var body = Encoding.UTF8.GetBytes(data);

            channel.BasicPublish(exchange: "AMQP-SERVER",
                                    routingKey: "",
                                    basicProperties: null,
                                    body: body);

            Console.WriteLine(" [x] Sent {0}", data);
        }

        public void Received(string droneID)
        {
            //open connection
            var factory = new ConnectionFactory() { Uri = new Uri(URI) };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received  '{0}'", message);
            };
            //listening on the queue with the same name as drone id
            channel.BasicConsume(queue: droneID,
                                autoAck: true,
                                consumer: consumer);
        }
    }
}
