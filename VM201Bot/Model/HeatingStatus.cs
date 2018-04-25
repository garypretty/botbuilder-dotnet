using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VM201Bot.Model
{
    public class HeatingStatus
    {
        public HeatingStatus(xml vm201Status)
        {
            Zones = new List<Zone>();

            var kitchen = new Zone()
            {
                IsOn = Convert.ToBoolean(vm201Status.status.leds[0].Value),
                Name = "Kitchen",
                ZoneNumber = 0
            };
            Zones.Add(kitchen);

            var downstairs = new Zone()
            {
                IsOn = Convert.ToBoolean(vm201Status.status.leds[1].Value),
                Name = "Downstairs",
                ZoneNumber = 1
            };
            Zones.Add(downstairs);

            var bedrooms = new Zone()
            {
                IsOn = Convert.ToBoolean(vm201Status.status.leds[2].Value),
                Name = "Bedrooms",
                ZoneNumber = 2
            };
            Zones.Add(bedrooms);

            var loft = new Zone()
            {
                IsOn = Convert.ToBoolean(vm201Status.status.leds[7].Value),
                Name = "Loft",
                ZoneNumber = 7
            };
            Zones.Add(loft);
        }

        public List<Zone> Zones { get; set; }

        public class Zone
        {
            public string Name { get; set; }
            public bool IsOn { get; set; }
            public int ZoneNumber { get; set; }
        }
    }
}
