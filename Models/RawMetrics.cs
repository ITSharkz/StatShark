using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatShark.Models
{
    public class RawMetrics
    {
        public double Total { get; set; }
        public double Used { get; set; }
        public double Free { get; set; }
        public string Unit { get; set; }
    }
}
