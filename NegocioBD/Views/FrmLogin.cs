using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NegocioBD.BAL;
using NegocioBD.DAL;

namespace AplicacionNegocio
{
    public partial class FrmLogin : Form
    {
        private Conexion conexionOracle = new Conexion();
        private static string sistema = "Sistema de Carros";
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string contrasena = txtContrasena.Text.Trim();
            string sistema = "Sistema Prueba"; 

            try
            {
                if (conexionOracle.ValidarAcceso(usuario, contrasena, sistema))
                {
                    int idUsuario = conexionOracle.ObtenerIdUsuario(usuario);
                    FrmCarros home = new FrmCarros(idUsuario,sistema);
                    home.Show();
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Acceso denegado: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
