﻿using System;
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

        public void Send(string droneID, List<SensorInterface> sensors)
        {
            httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint + "/drones");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            string data = JsonManager.getJsonString(droneID, sensors);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            Console.Out.WriteLine(httpResponse.StatusCode);

            httpResponse.Close();
        }

        public string Received(string droneID)
        {
            httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint+"/drones/"+ droneID + "/action");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            Console.Out.WriteLine(httpResponse.StatusCode);
            
            string json = new StreamReader(httpResponse.GetResponseStream()).ReadToEnd();
            
            httpResponse.Close();

            return json;
        }
    }
}
