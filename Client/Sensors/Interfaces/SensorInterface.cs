using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Sensors
{
    interface SensorInterface
    {
        //get sensor data in json
        string toJson();

        //get sensor name
        string getSensorName();

    }
}
