
using NegocioBD.BAL;
using NegocioBD.DAL;

namespace AplicacionNegocio
{
    
    public partial class FrmEditarCarro : Form
    {
        private Carro carro; // Objeto que representa el carro a editar
        private CarroBLL carroBLL = new CarroBLL(); //Instancia de la clase CarroBLL para manejar la lógica de negocio
        public string _sistema;
        public int _idUsuario;
        public FrmEditarCarro(int idUsuario, String sistema,Carro carro)
        {
            _sistema = sistema;
            _idUsuario = idUsuario;
            this.carro = carro;
            InitializeComponent();
            CargarDatosCarro();
        }// Fin del constructor

        //CargarDatosCarro: Método para cargar los datos del carro en los controles del formulario
        private void CargarDatosCarro()
        {
            txtMarca.Text = carro.Marca;
            txtModelo.Text = carro.Modelo;
            txtAnio.Text = carro.Año.ToString();
            txtPrecio.Text = carro.Precio.ToString("F2");
        }//Fin de CargarDatosCarro

        // btnVolver_Click: Método para manejar el evento de clic en el botón "Volver". Vuelve al formulario de carros sin guardar cambios.
        private void btnVolver_Click(object sender, EventArgs e)
        {
            FrmCarros frmCarros = new FrmCarros(_idUsuario,_sistema);
            frmCarros.Show();
            this.Close();
        }//Fin de btnVolver_click

        // btnCancelar_Click: Método para manejar el evento de clic en el botón "Cancelar". Pregunta al usuario si está seguro de cancelar la edición.
        // Retorna al formulario de carros si el usuario confirma la cancelación.
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"¿Está seguro que desea cancelar la edición para el carro {carro.Marca} {carro.Modelo}?",
                   "Cancelar edición", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                FrmCarros frmCarros = new FrmCarros(_idUsuario, _sistema);
                frmCarros.Show();
                this.Close();
            }// Fin de la validación de confirmación
            
        }// Fin de btnCancelar_Click

        // btnGuardar_Click: Método para manejar el evento de clic en el botón "Guardar".
        // Valida los campos del formulario y actualiza el carro si los datos son válidos.
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMarca.Text) ||
                string.IsNullOrWhiteSpace(txtModelo.Text) ||
                !int.TryParse(txtAnio.Text, out int anio) ||
                !decimal.TryParse(txtPrecio.Text, out decimal precio))
            {
                MessageBox.Show("Por favor completa todos los campos con datos válidos.");
                return;
            }// Fin de la validación de campos

            Carro carroActualizado = new Carro // Creación de un nuevo objeto Carro con los datos del formulario
            {
                Id = carro.Id,
                Marca = txtMarca.Text.Trim(),
                Modelo = txtModelo.Text.Trim(),
                Año = anio,
                Precio = precio
            };// Fin de la creación del objeto carroActualizado

            try
            {
                carroBLL.EditarCarro(carroActualizado); // Llamada al método EditarCarro de la clase CarroBLL para actualizar el carro
                MessageBox.Show("Carro actualizado correctamente.");
                FrmCarros frmCarros = new FrmCarros(_idUsuario, _sistema);
                frmCarros.Show();
                this.Close();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Error de validación: " + ex.Message, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el carro: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }//Fin del try-catch

        }//Fin de btnGuardar_Click
    }// Fin de la clase FrmEditarCarro
}//Fin del namespace AplicacionNegocio

