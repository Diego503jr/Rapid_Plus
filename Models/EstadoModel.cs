using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rapid_Plus.Models
{
    internal class EstadoModel
    {
        public int IdOrden { get; set; }
        public string Orden { get; set; }
        public int IdEstadoOrden { get; set; }
        public string EstadoOrden { get; set; }
    }
}
