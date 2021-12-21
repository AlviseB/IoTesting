using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Sensors
{
    class Timestamp : SensorInterface, TimestampInterface
    {
        public string toJson()
        {
            return "\"timestamp\": \"" + GetCurrentTimestamp() + "\"";
        }

        public string GetCurrentTimestamp()
        {
            String timeStamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            return timeStamp;
        }
    }
}
