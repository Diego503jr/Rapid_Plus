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
        public int Mesa { get; set; }
        public int IdEstadoOrden { get; set; }
        public string EstadoOrden { get; set; }

        public int IdPlatillo { get; set; }
        public string NombrePlatillo { get; set; }
        public string DescripcionPlatillo { get; set;}
        public decimal PrecioPlatillo { get; set; }


        public DateTime FechaOrden{ get; set; }
        public decimal Total { get; set; }
        public int UsuarioId { get; set; }

        public string NombreUsuario { get; set; }
        public decimal Subtotal { get; set; }
        

        public int IdCliente { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }

        public int IdCategoria { get; set; }

    }
}
