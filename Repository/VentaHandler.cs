using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MiPrimeraAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimeraAPI.Repository
{
    [ApiController]
    [Route("[Controller]")]
    public class VentaHandler : ControllerBase
    {
        public const string ConnectionString =
            "Server=DESKTOP-P6TBBSQ;" +
            "Initial Catalog=SistemaGestion;" +
            "Trusted_Connection=True";

        //CONSULTA DE VENTA
        [HttpGet(Name = "GetVenta")]
        public static List<Venta> GetVenta()
        {
            List<Venta> resultados = new List<Venta>();

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(
                    "SELECT * FROM Venta", sqlConnection))
                {
                    sqlConnection.Open();
                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        //Me aseguro que haya filas
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                Venta venta = new Venta();

                                venta.Id = Convert.ToInt32(dataReader["Id"]);
                                venta.comentarios = dataReader["comentarios"].ToString();
                               
                                resultados.Add(venta);
                            }
                        }
                    }
                    sqlConnection.Close();

                }
            }
            return resultados;
        }

        //ALTA DE VENTA
        [HttpPost(Name ="AltaVenta")]
        public static bool altaVentas(Venta venta)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryInsert = "INSERT INTO [SistemaGestion].[dbo].[Venta]" +
                    "(Comentarios) VALUES" +
                    "(@comentarios)";
                                
                SqlParameter comentariosParameter = new SqlParameter("ComentariosParameter", System.Data.SqlDbType.VarChar) { Value = venta.comentarios };
           
                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                {
                    sqlCommand.Parameters.Add(comentariosParameter);
                    
                    int numberOfRows = sqlCommand.ExecuteNonQuery();

                    if (numberOfRows > 0)
                    {
                        resultado = true;
                    }
                }
                sqlConnection.Close();
            }
            return resultado;
        }

        //MODIFICAR VENTA
        [HttpPut(Name ="ModificarVenta")]
        public static bool ModificarVenta(Venta venta)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryInsert = "UPDATE INTO [SistemaGestion].[dbo].[Venta]" +
                    "SET Comentarios = @comentarios" +
                    "WHERE Id=@id";

                SqlParameter idParameter = new SqlParameter("id", System.Data.SqlDbType.BigInt) { Value = venta.Id };
                SqlParameter comentariosParameter = new SqlParameter("Comentarios", System.Data.SqlDbType.VarChar) { Value = venta.comentarios };



                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                {
                    sqlCommand.Parameters.Add(idParameter);
                    sqlCommand.Parameters.Add(comentariosParameter);


                    int numberOfRows = sqlCommand.ExecuteNonQuery();

                    if (numberOfRows > 0)
                    {
                        resultado = true;
                    }
                }
                sqlConnection.Close();
            }
            return resultado;
        }

        //ELIMINAR VENTA
        [HttpDelete(Name ="EliminarVenta")]
        public static bool EliminarVenta(int id)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryDelete = "DELETE * FROM Venta WHERE Id=@id";

                SqlParameter sqlParameter = new SqlParameter("id", System.Data.SqlDbType.BigInt);
                sqlParameter.Value = id;

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryDelete, sqlConnection))
                {
                    sqlCommand.Parameters.Add(sqlParameter);
                    int numberOfRows = sqlCommand.ExecuteNonQuery();

                    if (numberOfRows > 0)
                    {
                        resultado = true;
                    }
                }
                sqlConnection.Close();
            }
            return resultado;
        }
    }
}
