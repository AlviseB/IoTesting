﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Sensors
{
    class VirtualBattery
    {
        public string toJson()
        {
            return "\"battery\": " + GetBattery();
        }
        public int GetBattery()
        {
            var random = new Random();
            return random.Next(100);
        }
    }
}
