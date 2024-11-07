using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rapid_Plus.Models
{
    internal class PlatilloModel
    {
        public int PlatilloId { get; set; }
        public string Platillo { get; set; }
        public string Descripcion { get; set; }
        public int CategoriaId { get; set; }
        public string Categoria { get; set; }
        public decimal Precio { get; set; }
        public int EstadoId { get; set; }
        public string Estado { get; set; }        

    }
}
