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
        //Cliente
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }

        //ORDENES
        public int IdOrden { get; set; }

        public int IdUsuario { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaOrden { get; set; }
        public decimal Total { get; set; }

        public string Orden { get; set; }
        public int Mesa { get; set; }
        public int IdEstadoOrden { get; set; }
        public string EstadoOrden { get; set; }

       
        

        public string NombreUsuario { get; set; }
        public decimal Subtotal { get; set; }
        

        

        public int IdCategoria { get; set; }

        public int IdEstado { get; set; }

        public int IdDetalleOrden { get; set; }
        
        public int IdPlatilloOrden { get; set; }

        public int IdMesa {  get; set; }

        //PLATILLO
        public int IdPlatillo { get; set; }
        public string NombrePlatillo { get; set; }
        public string DescripcionPlatillo { get; set; }
        public decimal PrecioPlatillo { get; set; }

    }
}
