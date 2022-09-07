using Microsoft.Data.SqlClient;
using MiPrimeraAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimeraAPI.Repository
{
    public class ProductoVendidoHandler
    {
        public const string ConnectionString =
        "Server=DESKTOP-P6TBBSQ;" +
        "Initial Catalog=SistemaGestion;" +
        "Encrypt = False;" +
        "Trusted_Connection=True";

        //CONSULTA DE PRODUCTO VENDIDO
        public static List<ProductoVendido> GetProductoVendido()
        {
            List<ProductoVendido> resultados = new List<ProductoVendido>();

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(
                    "SELECT * FROM ProductoVendido", sqlConnection))
                {
                    sqlConnection.Open();
                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        //Me aseguro que haya filas
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                ProductoVendido productoVendido = new ProductoVendido();

                                productoVendido.Id = Convert.ToInt32(dataReader["Id"]);
                                productoVendido.Stock = Convert.ToInt32(dataReader["Stock"]);
                                productoVendido.IdProducto = Convert.ToInt32(dataReader["IdProducto"]);
                                productoVendido.IdVenta = Convert.ToInt32(dataReader["IdVenta"]);
                                
                                resultados.Add(productoVendido);
                            }
                        }
                    }
                    sqlConnection.Close();

                }
            }
            return resultados;
        }

        //CREAR PRODUCTOVENDIDO
        public static bool CrearProductoVendido(ProductoVendido productovendido)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryInsert = "INSERT INTO [SistemaGestion].[dbo].[ProductoVendido]" +
                    "(Stock) VALUES" +
                    "(@stockParameter)";

                SqlParameter IdParameter = new SqlParameter("IdParameter", System.Data.SqlDbType.VarChar) { Value = productovendido.Id };
                SqlParameter StockParameter = new SqlParameter("StockParameter", System.Data.SqlDbType.VarChar) { Value =productovendido.Stock };
                
                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                {
                    sqlCommand.Parameters.Add(IdParameter);
                    sqlCommand.Parameters.Add(StockParameter);
      
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

        public static bool VenderProductosIdUsuarios(List<Producto> listaProducto,int idUsuario)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                //Inserto una nueva venta
                 string queryInsert = "INSERT INTO [SistemaGestion].[dbo].[Venta]" +
                    "(Comentarios) VALUES" +
                    "(@comentariosParameter);SELECT SCOPE_IDENTITY();";
                        
                SqlParameter comentariosParameter = new SqlParameter("comentariosParameter", System.Data.SqlDbType.VarChar) { Value = idUsuario };
                

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                {
                    sqlCommand.Parameters.Add(comentariosParameter);

                    var numberOfRows = sqlCommand.ExecuteScalar();

                    if (numberOfRows != null)
                    {
                        //Parseamos el objeto,transformamos el objeto a decimal
                        decimal idVenta = (decimal)numberOfRows;
                        
                        string queryProductoVendido = "INSERT INTO [SistemaGestion].[dbo].[ProductoVendido]" +
                        "(Stock),(IdProducto),(IdVenta) Values" +
                        "(@stockParameter),(@idproducto),(@idventa)";

                        foreach (var producto in listaProducto)
                        {
                            using (SqlCommand cmdProductoVendido = 
                               new SqlCommand(queryProductoVendido, sqlConnection))
                            {
                                SqlParameter idParameterProducto = 
                                    new SqlParameter("idproducto", System.Data.SqlDbType.BigInt)
                                    { Value = producto.Id };

                                SqlParameter idParameterVenta = 
                                    new SqlParameter("idventa", System.Data.SqlDbType.BigInt) 
                                    { Value = idVenta };

                                SqlParameter stockParameter = 
                                    new SqlParameter("stockParameter", System.Data.SqlDbType.BigInt) 
                                    { Value =producto.Stock };

                                cmdProductoVendido.Parameters.Add(idParameterProducto);
                                cmdProductoVendido.Parameters.Add(idParameterVenta);
                                cmdProductoVendido.Parameters.Add(stockParameter);

                                int filaInsertadaProductoVendido = cmdProductoVendido.ExecuteNonQuery();

                                if (filaInsertadaProductoVendido > 0)
                                {
                                    var queryResult = ProductoHandler.GetProductos();
                                    
                                }
                            }
                            bool updateStock = ProductoHandler.UpdateStockbyId(producto.Id, producto.Stock);

                            if (updateStock)
                            {
                                Console.WriteLine("Stock Actualizado");
                            }
                            else
                            {
                                Console.Write("Ocurrio un error !!!");
                            }
                        }
                        resultado = true;
                    }
                }
                sqlConnection.Close();
            }
            return resultado;
        }
    }
}
