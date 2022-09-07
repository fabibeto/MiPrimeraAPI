using Microsoft.Data.SqlClient;
using MiPrimeraAPI.Controllers;
using MiPrimeraAPI.Model;
using System;
using System.Collections.Generic;

namespace MiPrimeraAPI.Repository
{
    public static class UsuarioHandler
    {
        public const string ConnectionString =
            "Server=DESKTOP-P6TBBSQ;" +
            "Initial Catalog=SistemaGestion;" +
            "Trusted_Connection=True";
        
        //CONSULTA DE USUARIOS
        public static List<Usuario> GetUsuarios()
        {
            List<Usuario> resultados = new List<Usuario>();
            
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(
                    "SELECT * FROM Usuario", sqlConnection))
                {
                    sqlConnection.Open();
                    using (SqlDataReader dataReader= sqlCommand.ExecuteReader())
                    {  
                        //Me aseguro que haya filas
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                Usuario usuario = new Usuario();

                                usuario.Id = Convert.ToInt32(dataReader["Id"]);
                                usuario.NombreUsuario = dataReader["Nombre"].ToString();
                                usuario.Apellido = dataReader["Apellido"].ToString();
                                usuario.Contraseña = dataReader["Contraseña"].ToString();
                                usuario.Mail = dataReader["Mail"].ToString();

                                resultados.Add(usuario);
                            }
                        }
                    }
                    sqlConnection.Close();

                }
            }
            return resultados;
        }

        public static bool ModificarUSuario(Controllers.DTOS.PutUsuario usuario)
        {
            throw new NotImplementedException();
        }

        //CREAR USUARIOS
        public static bool CrearUSuario(Usuario usuario)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryInsert = "INSET INTO [SistemaGestion].[dbo].[Usuario]" +
                    "(Nombre,Apellido,NombreUsuario,Contraseña,Mail) VALUES" +
                    "(@nombreParameter,@apellidoParameter,@nombreUsuarioParameter," +
                    "@contraseñaParameter,@mailParameter)";

                SqlParameter nombreParameter = new SqlParameter("NombreParameter", System.Data.SqlDbType.VarChar) { Value = usuario.Nombre };
                SqlParameter apellidoParameter = new SqlParameter("ApellidoParameter", System.Data.SqlDbType.VarChar) { Value = usuario.Apellido };
                SqlParameter nombreUsuarioParameter = new SqlParameter("NombreUsuarioParameter", System.Data.SqlDbType.VarChar) { Value = usuario.NombreUsuario };
                SqlParameter contraseñaParameter = new SqlParameter("ContraseñaParameter", System.Data.SqlDbType.VarChar) { Value = usuario.Contraseña };
                SqlParameter mailParameter = new SqlParameter("MailParameter", System.Data.SqlDbType.VarChar) { Value = usuario.Mail };

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                {
                    sqlCommand.Parameters.Add(nombreParameter);
                    sqlCommand.Parameters.Add(apellidoParameter);
                    sqlCommand.Parameters.Add(nombreUsuarioParameter);
                    sqlCommand.Parameters.Add(contraseñaParameter);
                    sqlCommand.Parameters.Add(mailParameter);

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

        //MODIFICAR USUARIO
        public static bool ModificarUSuario(Usuario usuario)
        {
        bool resultado = false;

         using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
         {
            string queryInsert = "UPDATE [SistemaGestion].[dbo].[Usuario]" +
                "SET Nombre = @nombre" +
                "WHERE Id=@id";

                SqlParameter idParameter = new SqlParameter("id", System.Data.SqlDbType.BigInt) { Value = usuario.Id };
                SqlParameter nombreParameter = new SqlParameter("Nombre", System.Data.SqlDbType.VarChar) { Value = usuario.Nombre };
            
            

            sqlConnection.Open();

            using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
            {
                sqlCommand.Parameters.Add(nombreParameter);
                sqlCommand.Parameters.Add(idParameter);
                

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

        //ELIMINAR USUARIOS
        public static bool EliminarUsuario(int id)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryDelete = "DELETE * FROM Usuario WHERE Id=@id";

                SqlParameter sqlParameter = new SqlParameter("id",System.Data.SqlDbType.BigInt);
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
