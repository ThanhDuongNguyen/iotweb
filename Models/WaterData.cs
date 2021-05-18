using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iotweb.Models
{
    public class WaterData
    {
        public int ID { get; set; }
        public string MessageId { get; set; }
        public DateTime Time { get; set; }
        public string Temperature { get; set; }
        public string Turbidity { get; set; }
        public string PH { get; set; }
        public string Waterflow { get; set; }
    }
}
