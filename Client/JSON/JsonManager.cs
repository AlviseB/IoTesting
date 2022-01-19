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
        static public string getJsonString(string droneID, List<SensorInterface> sensors)
        {
            string json = "{";
            //add drone ID in json string
            json += "\"ID\": \"" + droneID + "\"";
            //add sensors in json string
            foreach (SensorInterface sensor in sensors)
            {
                json += ", ";
                string json_sensor = sensor.toJson();
                json += json_sensor;
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
