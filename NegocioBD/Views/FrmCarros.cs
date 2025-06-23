using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using NegocioBD.BAL;
using NegocioBD.DAL;
using static System.Collections.Specialized.BitVector32;

namespace AplicacionNegocio
{
    public partial class FrmCarros : Form
    {
        private readonly CarroBLL carroBLL = new CarroBLL(); // Instancia de la clase CarroBLL para manejar la lógica de negocio
        private Conexion conexionOracle = new Conexion();
        public string _sistema;
        public int _idUsuario;
        private readonly string pantalla;
        public FrmCarros(int idUsuario, String sistema)
        {
            InitializeComponent();
            _idUsuario = idUsuario;
            _sistema = sistema;
            ConfigurarDataGridView(); // Configura las columnas del DataGridView para mostrar los datos de los carros
            VerificarPermisos();
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            pantalla = config["Pantalla1"];
            this.lblSistema.Text = _sistema;
        }

        private void ConfigurarDataGridView()
        {
            dataGridView1.Columns.Clear(); // Limpia las columnas existentes en el DataGridView antes de configurarlas

            // Agrega las columnas normales que muestran propiedades de Carro
            dataGridView1.AutoGenerateColumns = false;

            // Configura las columnas del DataGridView para mostrar los datos de los carros
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "ID",
                DataPropertyName = "Id",
                ReadOnly = true
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Placa",
                DataPropertyName = "placa",
                ReadOnly = true
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Marca",
                DataPropertyName = "Marca",
                ReadOnly = true
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Modelo",
                DataPropertyName = "Modelo",
                ReadOnly = true
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Año",
                DataPropertyName = "Año",
                ReadOnly = true
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Precio",
                DataPropertyName = "Precio",
                ReadOnly = true
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Precio con impuesto",
                DataPropertyName = "precioImpuesto",
                ReadOnly = true
            });

            // Columna con icono editar
            var editarCol = new DataGridViewImageColumn
            {
                Name = "Editar",
                HeaderText = "Editar",
                Image = Image.FromFile("Icons/editar-informacion.png"), // Ruta relativa
                Width = 50,
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };
            dataGridView1.Columns.Add(editarCol);

            // Columna con icono eliminar
            var eliminarCol = new DataGridViewImageColumn
            {
                Name = "Eliminar",
                HeaderText = "Eliminar",
                Image = Image.FromFile("Icons/eliminar.png"),
                Width = 60,
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };
            dataGridView1.Columns.Add(eliminarCol);
            // Evento click en celdas para botones
            dataGridView1.CellContentClick += DataGridView1_CellContentClick;
        }

        // CargarDatos: Método que carga los datos de los carros en el DataGridView.
        private void CargarDatos()
        {
            var lista = carroBLL.ObtenerCarros(); // Llama al método ObtenerCarros de la clase CarroBLL para obtener la lista de carros desde la base de datos
            dataGridView1.DataSource = lista; // Asigna la lista de carros al DataGridView para mostrar los datos
            LimpiarCampos(); // Limpia los campos de texto del formulario después de cargar los datos
        }

        // LimpiarCampos: Método que limpia los campos de texto del formulario.
        private void LimpiarCampos()
        {
            txtMarca.Text = "";
            txtModelo.Text = "";
            txtPrecio.Text = "";
            txtAnio.Text = "";
        }

        // ObtenerCarroDesdeCampos: Método que crea un objeto Carro a partir de los datos ingresados en los campos de texto del formulario.
        private Carro ObtenerCarroDesdeCampos()
        {
            return new Carro //Crea y devuelve un nuevo objeto Carro con los datos ingresados en los campos de texto
            {
                Marca = txtMarca.Text,
                Modelo = txtModelo.Text,
                Precio = decimal.TryParse(txtPrecio.Text, out var p) ? p : 0,
                Año = int.TryParse(txtAnio.Text, out var a) ? a : 0

            };
        }

        // btnAgregar_Click: Método que maneja el evento de clic en el botón "Agregar".
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            var carro = ObtenerCarroDesdeCampos(); // Crea un nuevo objeto Carro con los datos ingresados en los campos de texto

            try
            {
                carroBLL.AgregarCarro(carro); // Llama al método AgregarCarro de la clase CarroBLL para agregar el carro a la base de datos
                MessageBox.Show("Carro agregado.");
                CargarDatos(); // Recarga los datos del DataGridView para mostrar el nuevo carro agregado
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo agregar el carro: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void VerificarPermisos()
        {
            bool puedeConsultar = conexionOracle.VerificarPermisoUsuario(_idUsuario, _sistema, pantalla, "Consultar");
            bool puedeEditar = conexionOracle.VerificarPermisoUsuario(_idUsuario, _sistema, pantalla, "Modificar");
            bool puedeEliminar = conexionOracle.VerificarPermisoUsuario(_idUsuario, _sistema, pantalla, "Eliminar");
            bool puedeInsertar = conexionOracle.VerificarPermisoUsuario(_idUsuario, _sistema, pantalla, "Insertar");

            dataGridView1.Columns["Editar"].Visible = puedeEditar;
            dataGridView1.Columns["Eliminar"].Visible = puedeEliminar;

            btnAgregar.Visible = puedeInsertar;

            if (puedeConsultar)
            {
                CargarDatos();
                dataGridView1.Visible = true;
            }
            else
            {
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                dataGridView1.Visible = false;
            }
        }



        //DataGridView1_CellContentClick: Método que maneja el evento de clic en las celdas del DataGridView.
        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica si el clic fue en una fila válida y en una columna de acción (Editar o Eliminar)
            if (e.RowIndex < 0)
                return;

            var fila = dataGridView1.Rows[e.RowIndex]; // Obtiene la fila seleccionada
            Carro carro = (Carro)fila.DataBoundItem; // Obtiene el objeto Carro asociado a la fila

            // Verifica si la columna clickeada es de Editar
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Editar")
            {
                AbrirFormularioEditar(carro); // Abre el formulario de edición
            }
            else if (dataGridView1.Columns[e.ColumnIndex].Name == "Eliminar") // Verifica si la columna clickeada es de Eliminar
            {
                // Pregunta al usuario si está seguro de eliminar el carro
                if (MessageBox.Show($"¿Está seguro de eliminar el carro {carro.Marca} {carro.Modelo}?",
                    "Confirmar eliminación", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        carroBLL.EliminarCarro(carro.Id);// Llama al método EliminarCarro de la clase CarroBLL para eliminar el carro
                        MessageBox.Show("Carro eliminado correctamente.");
                        CargarDatos();// Recarga los datos del DataGridView
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar el carro: " + ex.Message);
                    }
                }
            }
        }// Fin de DataGridView1_CellContentClick

        //AbrirFormularioEditar: Método para abrir el formulario de edición de un carro seleccionado.
        private void AbrirFormularioEditar(Carro carro)
        {
            this.Hide(); // Oculta FrmCarros
            FrmEditarCarro formEditar = new FrmEditarCarro(_idUsuario, _sistema, carro);// Crea una instancia del formulario de edición y le pasa el carro seleccionado
            formEditar.Show(); // Abre el nuevo formulario


        }// Fin de AbrirFormularioEditar

        private void button1_Click(object sender, EventArgs e)
        {
            FrmLogin frm = new FrmLogin();
            frm.Show();
            this.Close();
        }
    }// Fin de la clase FrmCarros
}//Fin del namespace 
