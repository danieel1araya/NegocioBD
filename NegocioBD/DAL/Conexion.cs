using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace NegocioBD.DAL
{
    public class Conexion
    {
        private static string connectionString = "User Id=Proyecto;Password=seguridad;Data Source=localhost:1521/xepdb1;";

        public bool ValidarCredenciales(string usuario, string contrasena)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    using (OracleCommand command = new OracleCommand("SP_ValidarCredenciales", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new OracleParameter("Usuario", OracleDbType.Varchar2, usuario, ParameterDirection.Input));
                        command.Parameters.Add(new OracleParameter("Contrasena", OracleDbType.Varchar2, contrasena, ParameterDirection.Input));

                        OracleParameter resultadoParam = new OracleParameter("Resultado", OracleDbType.Int32);
                        resultadoParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(resultadoParam);

                        command.ExecuteNonQuery();

                        int resultado = Convert.ToInt32(resultadoParam.Value.ToString());

                        return resultado == 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar credenciales: " + ex.Message);
            }
        }

        public int ObtenerIdUsuario(string nombreUsuario)
        {
            int idUsuario = -1;

            try
            {
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT IdUsuario FROM Usuarios WHERE Usuario = :usuario";

                    using (OracleCommand cmd = new OracleCommand(query, connection))
                    {
                        cmd.Parameters.Add("usuario", OracleDbType.Varchar2).Value = nombreUsuario;

                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            idUsuario = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener IdUsuario: " + ex.Message);
            }
            return idUsuario;
        }

        public bool ValidarAcceso(string usuario, string contrasena, string sistema)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    using (OracleCommand cmd = new OracleCommand("SP_ValidarAccesoSistema", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetros de entrada
                        cmd.Parameters.Add("p_usuario", OracleDbType.Varchar2).Value = usuario;
                        cmd.Parameters.Add("p_contrasena", OracleDbType.Varchar2).Value = contrasena;
                        cmd.Parameters.Add("p_nombre_sistema", OracleDbType.Varchar2).Value = sistema;

                        // Parámetros de salida
                        cmd.Parameters.Add("p_resultado", OracleDbType.Int32).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("p_mensaje", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;

                        // Ejecutar el procedimiento
                        cmd.ExecuteNonQuery();

                        int resultado = int.Parse(cmd.Parameters["p_resultado"].Value.ToString());
                        string mensaje = cmd.Parameters["p_mensaje"].Value.ToString();

                        if (resultado == 1)
                        {
                            return true;
                        }
                        else
                        {
                            throw new Exception(mensaje);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar acceso: " + ex.Message);
            }
        }


        public bool VerificarPermisoUsuario(int idUsuario, string nombreSistema, string nombrePantalla, string nombrePermiso)
        {
            try
            {
                using (var connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    using (var cmd = new OracleCommand("SP_VerificarPermisoUsuario", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetros de entrada
                        cmd.Parameters.Add("p_id_usuario", OracleDbType.Int32).Value = idUsuario;
                        cmd.Parameters.Add("p_nombre_sistema", OracleDbType.Varchar2).Value = nombreSistema;
                        cmd.Parameters.Add("p_nombre_pantalla", OracleDbType.Varchar2).Value = nombrePantalla;
                        cmd.Parameters.Add("p_nombre_permiso", OracleDbType.Varchar2).Value = nombrePermiso;

                        // Parámetro de salida
                        cmd.Parameters.Add("p_tiene_permiso", OracleDbType.Int32).Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();

                        int resultado = Convert.ToInt32(cmd.Parameters["p_tiene_permiso"].Value.ToString());
                        return resultado == 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar permiso del usuario: " + ex.Message);
            }
        }
    }
}
