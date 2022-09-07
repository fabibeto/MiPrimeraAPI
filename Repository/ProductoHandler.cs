using Microsoft.Data.SqlClient;
using MiPrimeraAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace MiPrimeraAPI.Repository
{
    public static class ProductoHandler
    {
     
        public const string ConnectionString =
        "Server=DESKTOP-P6TBBSQ;" +
        "Initial Catalog=SistemaGestion;" +
        "Encrypt = False;"+
        "Trusted_Connection=True";


       //CONSULTA DE PRODUCTOS
        public static List<Producto> GetProductos()
        {
            List<Producto> resultados = new List<Producto>();

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(
                    "SELECT * FROM Producto", sqlConnection))
                {
                    sqlConnection.Open();
                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        //Me aseguro que haya filas
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                Producto producto = new Producto();

                                producto.Id = Convert.ToInt32(dataReader["Id"]);
                                producto.Descripciones = dataReader["Descripciones"].ToString();
                                producto.Costo = Convert.ToInt32(dataReader["Costo"]);
                                producto.PrecioVenta = Convert.ToInt32(dataReader["PrecioVenta"]);
                                producto.Stock = Convert.ToInt32(dataReader["Stock"]);
                                producto.IdUsuario = Convert.ToInt32(dataReader["IdUSuario"]);


                                resultados.Add(producto);
                            }
                        }
                    }
                    sqlConnection.Close();

                }
            }
            return resultados;
        }

        //ALTA DE PRODUCTOS
        public static bool altaProductos(Producto producto)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryInsert = "INSET INTO [SistemaGestion].[dbo].[Prodcuto]" +
                    "(Descripciones,Costo,PrecioVenta,Stock) VALUES" +
                    "(@descripcionesParameter,@costoParameter,@precioVentaParameter," +
                    "@stockParameter)";

                SqlParameter descripcionesParameter = new SqlParameter("DescripcionesParameter", System.Data.SqlDbType.VarChar) { Value = producto.Descripciones};
                SqlParameter costoParameter = new SqlParameter("CostoParameter", System.Data.SqlDbType.VarChar) { Value = producto.Costo};
                SqlParameter precioVentaParameter = new SqlParameter("PrecioVentaParameter", System.Data.SqlDbType.VarChar) { Value = producto.PrecioVenta};
                SqlParameter stockParameter = new SqlParameter("StockParameter", System.Data.SqlDbType.VarChar) { Value = producto.Stock};
                
                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                {
                    sqlCommand.Parameters.Add(descripcionesParameter);
                    sqlCommand.Parameters.Add(costoParameter);
                    sqlCommand.Parameters.Add(precioVentaParameter);
                    sqlCommand.Parameters.Add(stockParameter);
                    
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

        //MODIFICAR PRODUCTOS
        public static bool ModificarProducto(Producto producto)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryInsert = "UPDATE [SistemaGestion].[dbo].[Producto]" +
                    "SET Descripciones = @descripciones" +
                    "WHERE Id=@id";

                SqlParameter idParameter = new SqlParameter("id", System.Data.SqlDbType.BigInt) { Value = producto.Id };
                SqlParameter descripcionesParameter = new SqlParameter("Descripciones", System.Data.SqlDbType.VarChar) { Value = producto.Descripciones};



                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                {
                    sqlCommand.Parameters.Add(idParameter);
                    sqlCommand.Parameters.Add(descripcionesParameter);


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


        //ELIMINAR PRODUCTOS
        public static bool EliminarProducto(int id)
        {
            bool resultado = false;
            List<int> resultados = new List<int>();

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string query = "SELECT PV.Id FROM Producto AS P "+
                                " INNER JOIN ProductoVendido AS PV ON "+
                                " P.Id = PV.IdProducto  WHERE PV.IdProducto = @id";
                
                SqlParameter idParameter = new SqlParameter("id", System.Data.SqlDbType.BigInt) { Value =id};

        
                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(query,sqlConnection))
                {
                    sqlCommand.Parameters.Add(idParameter);

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {

                        //Me aseguro que haya filas
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                var idProductoVendido = Convert.ToInt32(dataReader["Id"]);

                                resultados.Add(idProductoVendido);
                            }
                        }
                    }
                    
                    string queryDelete = "DELETE FROM ProductoVendido WHERE Id=@id";
                    
                    foreach (var idProductoVendidoDelete in resultados)
                    {
                      using (SqlCommand sqlCommandDelete = new SqlCommand(queryDelete, sqlConnection))
                      {
                            SqlParameter idParameterProductoVendido = new SqlParameter("id", System.Data.SqlDbType.BigInt) { Value = idProductoVendidoDelete };
                            sqlCommandDelete.Parameters.Add(idParameterProductoVendido);
                            int numberOfRows = sqlCommandDelete.ExecuteNonQuery();
                      }
                    }

                    string queryDeleteProducto = "DELETE FROM Producto WHERE Id=@id";
                    
                    using (SqlCommand sqlCommandDeleteProducto = new SqlCommand(queryDeleteProducto, sqlConnection))
                    {
                        SqlParameter idParameterProducto = new SqlParameter("id", System.Data.SqlDbType.BigInt) { Value = id};
                        
                        sqlCommandDeleteProducto.Parameters.Add(idParameterProducto);
                        int numberOfRows = sqlCommandDeleteProducto.ExecuteNonQuery();

                        if (numberOfRows > 0)
                        {
                            resultado=true;
                        }
                    }
                     
                }
                sqlConnection.Close();
                return resultado;
            }

            using (SqlCommand command=new SqlCommand())
            //1_Verifiar si el producto esta vendido
            //  Get ProductoVendido
            //2_

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryDelete = "DELETE * FROM Producto WHERE Id=@id";

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
