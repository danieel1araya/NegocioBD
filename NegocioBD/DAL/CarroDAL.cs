using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NegocioBD.BAL;

namespace NegocioBD.DAL
{
    public static class CarroDAL
    {
        //string de conexión a la base de datos SQL Server
        private static string connectionString = "Server=LENOVODANIEL\\SQLEXPRESS;Database=AplicacionCarros;Trusted_Connection=True;";

        // Método para obtener todos los carros desde la base de datos
        public static List<Carro> ObtenerTodos()
        {
            List<Carro> lista = new List<Carro>(); // Inicializa una lista vacía de objetos Carro para almacenar los resultados de la consulta a la base de datos.

            using (SqlConnection con = new SqlConnection(connectionString))// Crea una nueva conexión a la base de datos utilizando la cadena de conexión definida anteriormente.
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Carros", con); // Crea un nuevo comando SQL para seleccionar todos los registros de la tabla Carros.
                con.Open(); // Abre la conexión a la base de datos para ejecutar el comando SQL.
                SqlDataReader reader = cmd.ExecuteReader(); // Ejecuta el comando SQL y obtiene un SqlDataReader para leer los resultados de la consulta.
                while (reader.Read()) // Mientras haya registros disponibles en el SqlDataReader, se itera sobre cada registro.
                {
                    Carro c = new Carro // Crea una nueva instancia de la clase Carro y asigna los valores de las columnas del registro actual a las propiedades del objeto Carro.
                    {
                        Id = (int)reader["Id"], // Obtiene el valor de la columna "Id" y lo convierte a entero.
                        placa = reader["Placa"].ToString(), // Obtiene el valor de la columna "Placa" y lo convierte a cadena de texto.
                        Marca = reader["Marca"].ToString(), // Obtiene el valor de la columna "Marca" y lo convierte a cadena de texto.
                        Modelo = reader["Modelo"].ToString(), // Obtiene el valor de la columna "Modelo" y lo convierte a cadena de texto.
                        Año = (int)reader["Año"], // Obtiene el valor de la columna "Año" y lo convierte a entero.
                        Precio = (decimal)reader["Precio"], // Obtiene el valor de la columna "Precio" y lo convierte a decimal.
                        precioImpuesto = reader["precioImpuestos"] == DBNull.Value ? 0 : (decimal)reader["precioImpuestos"], // Obtiene el valor de la columna "precioImpuestos", si es DBNull, asigna 0, de lo contrario lo convierte a decimal.
                        FechaRegistro = (DateTime)reader["FechaRegistro"] // Obtiene el valor de la columna "FechaRegistro" y lo convierte a DateTime.
                    };
                    lista.Add(c); // Agrega el objeto Carro creado a la lista de carros.
                }

            }

            return lista; // Retorna la lista de carros obtenida de la base de datos
        }// Fin de método ObtenerTodos

        // Agregar: Método para agregar un nuevo carro utilizando un procedimiento almacenado
        public static void Agregar(Carro c)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString)) // Crea una nueva conexión a la base de datos usando la cadena de conexión definida.
                {
                    SqlCommand cmd = new SqlCommand("InsertarCarro", con);// Crea un nuevo comando SQL para ejecutar el procedimiento almacenado "InsertarCarro".
                    cmd.CommandType = CommandType.StoredProcedure;// Especifica que el comando es un procedimiento almacenado.
                    cmd.Parameters.AddWithValue("@Marca", c.Marca);// Agrega un parámetro al comando con la marca del carro a insertar.
                    cmd.Parameters.AddWithValue("@Modelo", c.Modelo); // Agrega un parámetro al comando con el modelo del carro a insertar.
                    cmd.Parameters.AddWithValue("@Año", c.Año); // Agrega un parámetro al comando con el año del carro a insertar.
                    cmd.Parameters.AddWithValue("@Precio", c.Precio); // Agrega un parámetro al comando con el precio del carro a insertar.
                    con.Open(); // Abre la conexión a la base de datos.
                    cmd.ExecuteNonQuery(); // Ejecuta el comando SQL, que insertará un nuevo carro en la base de datos.
                }
            }
            catch (SqlException ex)
            {

                throw;

            }
        }// Fin de método Agregar

        public static bool Editar(Carro c)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))//Crea una nueva conexión a la base de datos usando la cadena de conexión definida.
                {
                    SqlCommand cmd = new SqlCommand("ActualizarCarro", con); //Crea un nuevo comando SQL para ejecutar el procedimiento almacenado "ActualizarCarro".
                    cmd.CommandType = CommandType.StoredProcedure;// Especifica que el comando es un procedimiento almacenado.
                    cmd.Parameters.AddWithValue("@Id", c.Id);// Agrega un parámetro al comando con el Id del carro a actualizar.
                    cmd.Parameters.AddWithValue("@Marca", c.Marca);// Agrega un parámetro al comando con la marca del carro a actualizar.
                    cmd.Parameters.AddWithValue("@Modelo", c.Modelo);// Agrega un parámetro al comando con el modelo del carro a actualizar.
                    cmd.Parameters.AddWithValue("@Año", c.Año);// Agrega un parámetro al comando con el año del carro a actualizar.
                    cmd.Parameters.AddWithValue("@Precio", c.Precio);// Agrega un parámetro al comando con el precio del carro a actualizar.
                    con.Open();// Abre la conexión a la base de datos.
                    int filasAfectadas = cmd.ExecuteNonQuery(); // Ejecuta el comando SQL, que actualizará el carro con el Id especificado y devuelve el número de filas afectadas.
                    return filasAfectadas > 0;  // true si se actualizó al menos un registro
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }// Fin de método Editar


        // Eliminar: Método para eliminar un carro por su Id utilizando un procedimiento almacenado
        public static void Eliminar(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString)) //// Crea una nueva conexión a la base de datos usando la cadena de conexión definida.
            {
                SqlCommand cmd = new SqlCommand("EliminarCarro", con); //Crea un nuevo comando SQL para ejecutar el procedimiento almacenado "EliminarCarro".
                cmd.CommandType = CommandType.StoredProcedure; // Especifica que el comando es un procedimiento almacenado.
                cmd.Parameters.AddWithValue("@Id", id); // Agrega un parámetro al comando con el Id del carro a eliminar.
                con.Open(); //Abre la conexión a la base de datos.
                cmd.ExecuteNonQuery(); // Ejecuta el comando SQL, que eliminará el carro con el Id especificado.
            }
        }//Fin de método Eliminar

    }//Fin de clase CarroDAL
}
