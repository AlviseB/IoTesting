﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Sensors
{
    class VirtualGPSSensor : SensorInterface, GPSSensorInterface
    {
        public string toJson()
        {
            return "gps: {\"latitude\": " + GetLatitude() + ", \"longitude\": " + GetLongitude() + "}";
        }

        public double GetLatitude()
        {
            var random = new Random();
            return Math.Round(random.NextDouble() * (90 - (-90)) + (-90), 8);
        }
        public double GetLongitude()
        {
            var random = new Random();
            return Math.Round(random.NextDouble() * (180 - (-180)) + (-180), 8);
        }
    }
}
