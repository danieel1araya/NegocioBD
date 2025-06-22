using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegocioBD.BAL
{
    public class Carro
    {
        public int Id { get; set; }
        public string placa { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Año { get; set; }
        public decimal Precio { get; set; }
        public decimal precioImpuesto { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
