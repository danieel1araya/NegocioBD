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
using Microsoft.Extensions.Configuration;

namespace AplicacionNegocio
{
    public partial class FrmLogin : Form
    {
        private Conexion conexionOracle = new Conexion();
        private readonly string sistema;
        public FrmLogin()
        {
            InitializeComponent();
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            sistema = config["Sistema"];
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string contrasena = txtContrasena.Text.Trim();


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
