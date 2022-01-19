using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Sensors
{
    class VirtualOrientation : SensorInterface, OrientationInterface
    {
        public string getSensorName()
        {
            return "orientation";
        }

        public string toJson()
        {
            return "\"orientation\": [" + GetX() + "," + GetY() + "," + GetZ() + "]";
        }

        public int GetX()
        {
            var random = new Random();
            return random.Next(181);
        }
        public int GetY()
        {
            var random = new Random();
            return random.Next(181);
        }
        public int GetZ()
        {
            var random = new Random();
            return random.Next(181);
        }
    }
}
