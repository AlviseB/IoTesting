using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Sensors
{
    class VirtualAltitudeSensor : AltitudeSensorInterface, SensorInterface
    {
        public string getSensorName()
        {
            return "altitude";
        }

        public string toJson()
        {
            return "\"altitude\": " + GetAltitude();
        }

        public int GetAltitude()
        {
            var random = new Random();
            return random.Next(2000);
        }
    }
}
