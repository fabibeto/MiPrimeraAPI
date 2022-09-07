using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimeraAPI.Controllers.DTOS
{
    public class PostUsuario
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NombreUsuario { get; set; }
        public string Contraseña  { get; set; }
        public String mail { get; set; }
        public string NombreApellido {
            get {
                return Nombre + " " + Apellido;
            }
        }

    }
}
