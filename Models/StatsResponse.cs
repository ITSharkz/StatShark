using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatShark.Models
{
    public class StatsResponse
    {
        public string? HostId { get; set; }
        public DateTime DateTime { get; set; }
        public RawMetrics Ram { get; set; }
        public RawMetrics Cpu { get; set; }
        public RawMetrics Disk { get; set; }


    }
}
