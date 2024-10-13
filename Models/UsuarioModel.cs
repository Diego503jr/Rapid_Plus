using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rapid_Plus.Models
{
    class UsuarioModel
    {
        public int UsuarioId { get; set; }
        public string Usuario { get; set; }
        public string Clave { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public int RolId { get; set; }
        public string Rol { get; set; }
        public int DUI { get; set; }
        public int SexoId { get; set; }
        public string Sexo { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int EstadoId { get; set; }
        public int ContactoId { get; set; }
        public string Telefono1 { get; set; }
        public string Telefono2 { get; set; }
        public string Estado { get; set; }
    }
}
