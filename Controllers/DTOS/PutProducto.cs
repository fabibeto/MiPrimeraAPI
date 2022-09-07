using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimeraAPI.Controllers.DTOS
{
    public class PutProducto
    {
        public int Id { get; set; }
        public string Descripciones { get; set; }
        public int Costo { get; set; }
        public int PrecioVenta { get; set; }
        public int Stock { get; set; }
    }
}
