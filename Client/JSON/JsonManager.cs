using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Sensors;
using Newtonsoft.Json;

namespace Client.JSON
{
    static class JsonManager
    {
        static public string assembleJSON(string droneID, Dictionary<string, string> sensors)
        {
            string json = "{";
            //add drone ID in json string
            json += "\"ID\": \"" + droneID + "\"";

            //add sensors in json string
            foreach (var sensor in sensors)
            {
                json += ", ";
                string data = sensor.Value;
                json += data;
            }
            json += '}';

            return json;
        }

        static public string deserializeCommand(string json)
        {
            Dictionary<string, string> action = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            return action["command"];
        }
    }
}
