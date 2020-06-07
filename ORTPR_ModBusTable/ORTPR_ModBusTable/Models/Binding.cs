using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORTPR_ModBusTable.Models
{
    class Binding
    {
        /// <summary>
        /// Полное наименование тега
        /// </summary>
        public string Tag { get; set; }
        public string Type { get; set; }
        public int Address { get; set; }

        public Binding(string tag, string type, int address)
        {
            Tag = tag;
            Type = type;
            Address = address;
        }
    }
}
