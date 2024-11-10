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

        //ORDENES
        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaOrden { get; set; }
        public decimal Total { get; set; }
        public int Mesa { get; set; }
        public int IdEstadoOrden { get; set; }
        public string Orden { get; set; }
        public int IdOrden { get; set; }
        public int IdPlatilloOrden { get; set; }
        public int IdDetalleOrden { get; set; }
        public string NombrePlatillo { get; set; }
        public string DescripcionPlatillo { get; set; }
        public int IdMesa { get; set; }
        public int Cantidad { get; set; }
        public string EstadoOrden { get; set; }
        public string NombreUsuario { get; set; }


        //ATRIBUTOS PARA CAJERO
        public decimal Subtotal { get; set; }
        public decimal PrecioPlatillo { get; set; }
        public string NombreCliente { get; set; }

    }
}
