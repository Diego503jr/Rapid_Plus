using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rapid_Plus.Models.Mesero
{
    class OrdenesModel
    {

        //Atributos
        //Número de orden, orden, número de mesa, estado
        public int IdOrden { get; set; }
        public int Cantidad { get; set; }
        public string Orden { get; set; }
        public string Mesa { get; set; }
        public string EstadoOrden { get; set; }

    }
}
