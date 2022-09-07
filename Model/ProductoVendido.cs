using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimeraAPI.Model
{
    public class ProductoVendido
    {
        public int Id { get; set; }
        public int Stock { get; set; }
        public virtual int IdProducto { get; set; }
        public virtual int IdVenta { get; set; }
    }
}
