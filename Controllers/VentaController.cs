using Microsoft.AspNetCore.Mvc;
using MiPrimeraAPI.Controllers.DTOS;
using MiPrimeraAPI.Model;
using MiPrimeraAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimeraAPI.Controllers
{
    public class VentaController:ControllerBase
    {
        [HttpGet(Name = "GetVenta")]
        public List<Venta> GetVenta()
        {
            return VentaHandler.GetVenta();
        }

        [HttpPost(Name = "PostVenta")]
        public bool altaVentas([FromBody] Venta venta)
        {
            return VentaHandler.altaVentas(new Venta
            {
                comentarios = venta.comentarios
            });
        }

        [HttpPut(Name ="PutVenta")]
        public bool ModificarVenta([FromBody] PutVenta venta)
        {
            return VentaHandler.ModificarVenta(new Venta
            {
                Id = venta.Id,
                comentarios= venta.Comentarios
            });
        }

        [HttpDelete (Name ="Delete")]
        public bool EliminarVenta([FromBody] int id)
        {
            try
            {
                return VentaHandler.EliminarVenta(id);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return false;
            }

        }
    }
}
