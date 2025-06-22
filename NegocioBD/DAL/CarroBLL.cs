using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NegocioBD.BAL;

namespace NegocioBD.DAL
{
    public class CarroBLL
    {

        public List<Carro> ObtenerCarros() // Método que retorna una lista de todos los carros desde carroDAL
        {
            return CarroDAL.ObtenerTodos();
        }

        // Método para agregar un carro con validaciones
        public void AgregarCarro(Carro c)
        {
            var errores = new List<string>(); // Lista para almacenar errores de validación

            if (string.IsNullOrWhiteSpace(c.Marca))
                errores.Add("La marca es requerida.");

            if (string.IsNullOrWhiteSpace(c.Modelo))
                errores.Add("El modelo es requerido.");

            if (c.Año <= 0)
                errores.Add("El año es requerido y debe ser válido.");

            if (c.Año > DateTime.Now.Year)
                errores.Add("El año no puede ser mayor al año actual.");

            if (c.Precio <= 0)
                errores.Add("El precio debe ser mayor que cero.");

            if (errores.Any())
                throw new ArgumentException(string.Join(" ", errores));

            try
            {
                CarroDAL.Agregar(c); // Agrega el carro a la base de datos utilizando CarroDAL
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar el carro: " + ex.Message);
            }
        }


        // Método para editar un carro con validaciones
        public bool EditarCarro(Carro c)
        {
            if (c.Id <= 0)
                throw new ArgumentException("El Id del carro no es válido.");
            if (string.IsNullOrWhiteSpace(c.Marca))
                throw new ArgumentException("La marca es requerida.");
            if (string.IsNullOrWhiteSpace(c.Modelo))
                throw new ArgumentException("El modelo es requerido.");
            if (c.Año > DateTime.Now.Year)
                throw new ArgumentException("El año no puede ser mayor al año actual.");
            if (c.Precio <= 0)
                throw new ArgumentException("El precio debe ser mayor que cero.");

            try
            {
                return CarroDAL.Editar(c); // Edita el carro en la base de datos utilizando CarroDAL
            }
            catch (Exception ex)
            {
                throw new Exception("Error al editar el carro: " + ex.Message);
            }
        }

        // Método para eliminar un carro por su Id
        public void EliminarCarro(int id)
        {
            if (id <= 0) // Verifica si el Id del carro es válido
                throw new ArgumentException("El Id del carro no es válido.");

            try
            {
                CarroDAL.Eliminar(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el carro: " + ex.Message);
            }
        }
    }
}
