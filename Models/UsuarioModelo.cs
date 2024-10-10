using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rapid_Plus.Models
{
    class UsuarioModelo
    {
        public int UsuarioId { get; set; }
        public string Usuario { get; set; }
        public string Clave { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public int RolId { get; set; }
        public int DUI { get; set; }
        public int SexoId {get; set;}
        public string FechaNacimiento { get; set; }
    }
}
