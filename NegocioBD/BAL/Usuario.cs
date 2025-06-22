using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegocioBD.BAL
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Contrasena { get; set; }
        public int Activo { get; set; }

    }
}
