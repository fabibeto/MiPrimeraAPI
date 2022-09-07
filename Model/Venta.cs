using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimeraAPI.Model
{
    public class Venta
    {
        public int Id { get; set; }
        public string comentarios { get; set; }

        public virtual ICollection<ProductoVendido> Productos { get; set; }
    }
}
