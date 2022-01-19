using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Client.Sensors;
using Client.JSON;

namespace Client.Protocols
{
    class Http : ProtocolInterface
    {
        private string endpoint;
        private HttpWebRequest httpWebRequest;

        public Http(string endpoint)
        {
            this.endpoint = endpoint;
        }

        public void Send(string droneID, Dictionary<string, string> sensors)
        {
            httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint + "/drones");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            string data = JsonManager.assembleJSON(droneID, sensors);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            Console.Out.WriteLine(httpResponse.StatusCode);

            httpResponse.Close();
        }

        public void Received(string droneID)
        {
            while (true)
            {
                //get JSON
                httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint+"/drones/"+ droneID + "/action");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Console.Out.WriteLine(httpResponse.StatusCode);
            
                string json = new StreamReader(httpResponse.GetResponseStream()).ReadToEnd();
            
                httpResponse.Close();

                //deserialize and print 
                try
                {
                    string command = JsonManager.deserializeCommand(json);
                    Console.WriteLine("Command : " + command);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Receiving error: " + ex);
                }
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
