using System;

namespace ARArt.Models
{
    /// <summary>
    /// Eigenschaften eines einzelen iBeacons. 
    /// </summary>
    public class Beacon
    {
        public string Name { get; set; }

        public string Uuid { get; set; }

        public int Major { get; set; }

        public int Minor { get; set; }

        public double Accuracy { get; set; }

        public int Rssi { get; set; }

        public string Proximity { get; set; }

        public Beacon()
        {

        }
    }
}

