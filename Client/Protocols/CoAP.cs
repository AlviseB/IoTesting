using Client.Sensors;
using System.Collections.Generic;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoAPNet;
using CoAPNet.Udp;
using Client.JSON;

namespace Client.Protocols
{
    class CoAP : ProtocolInterface
    {
        private string endpoint;
        private CoapClient client;
        private CancellationTokenSource cancellationTokenSource;
        public CoAP(string endpoint)
        {
            this.endpoint = endpoint;
            // Create a new client using a UDP endpoint (defaults to 0.0.0.0 with any available port number)
            client = new CoapClient(new CoapUdpEndPoint());
            // Create a cancelation token that cancels after 1 minute
            cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(1));
        }

        public async void Send(string droneID, List<SensorInterface> sensors)
        {
            string data = JsonManager.getJsonString(droneID, sensors);

            try
            {
                var message = new CoapMessage
                {
                    Code = CoapMessageCode.Post,
                    Type = CoapMessageType.NonConfirmable,  //non confirmable, non mi interessa ricevere una risposta, accetto che possa perdere dati ogni tanto a favore di velocità e leggerezza
                    Payload = Encoding.UTF8.GetBytes(data),
                };

                // Get the /hello resource from localhost.
                message.SetUri("coap://"+endpoint+"/drones");

                Console.WriteLine($"Sending a {message.Code} {message.GetUri().GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped)} request");
                await client.SendAsync(message, cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception caught: {ex}");
            }
        }

        public async void Received(string droneID)
        {
            try
            {
                var message = new CoapMessage
                {
                    Code = CoapMessageCode.Get,
                    Type = CoapMessageType.Confirmable,
                };

                // Get the /hello resource from localhost.
                message.SetUri("coap://" + endpoint + "/drones/"+droneID+"/action");

                Console.WriteLine($"Sending a {message.Code} {message.GetUri().GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped)} request");
                await client.SendAsync(message, cancellationTokenSource.Token);

                // Wait for the server to respond.
                var response = await client.ReceiveAsync(cancellationTokenSource.Token);

                string payload = Encoding.UTF8.GetString(response.Message.Payload);

                string command = JsonManager.deserializeCommand(payload);
                Console.WriteLine("Command : " + command);


                // Output our response
                Console.WriteLine($"Received a command from {response.Endpoint}\n{Encoding.UTF8.GetString(response.Message.Payload)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception caught: {ex}");
            }
        }
    }
}
