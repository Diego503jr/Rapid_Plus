using Rapid_Plus.Controllers;
using Rapid_Plus.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Rapid_Plus.Views.Administrador
{
    /// <summary>
    /// Lógica de interacción para Menu.xaml
    /// </summary>
    public partial class Menu : Page
    {
        private DispatcherTimer timer;
        public Menu()
        {
            InitializeComponent();
            IniciarTemporizador();
            CargarEstados();
            CargarCategorias();
        }

        #region VARIABLES LOCALES
        private static string conexion = Properties.Settings.Default.DbRapidPlus; 
        //Conexion a la DB
        SqlConnection conDB = new SqlConnection(conexion);

        //Variables de estado
        private bool agregar = false, editar = false;

        //Variable para almacenar el id
        private int idPlatillo = 0;
        //Variable para almaccenar el idEstado
        private int idEstado = 0;

        #endregion

        #region METODOS PERSONALIZADOS

        private void IniciarTemporizador() 
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += Timer_Tik;
        }

        private void Timer_Tik(object sender, EventArgs e) 
        {
            MostrarMenu();
            CargarCategorias();
        }

        //Validar formulario 
        bool ValidarFormulario() 
        {
            bool estado = true;
            string msj = null;

            if (string.IsNullOrEmpty(txtNombrePlatillo.Text)) 
            {
                estado = false;
                msj = "Nombre Platillo\n";

            }

            if (string.IsNullOrEmpty(txtDescripicion.Text))
            {
                estado = false;
                msj = "Descripcion Platillo\n";

            }

            if (string.IsNullOrEmpty(cmbCategoria.Text))
            {
                estado = false;
                msj = "Categoria Platillo\n";

            }

            if (string.IsNullOrEmpty(txtPrecio.Text))
            {
                estado = false;
                msj = "Precio Platillo\n";

            }

            if (string.IsNullOrEmpty(cmbEstado.Text) && idEstado == 0)
            {
                estado = false;
                msj = "Estado Platillo\n";

            }

            if (!estado) 
            {
                MessageBox.Show("Debe cumplir estos campos:\n" + msj,
               "Validacion de formulario", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return estado;
        }

        //Habilitar formulario
        void HabilitarFormulario(bool accion) 
        { 
            txtNombrePlatillo.IsEnabled = accion;
            txtDescripicion.IsEnabled = accion;
            cmbCategoria.IsEnabled = accion;
            lbDolar.IsEnabled = accion;
            txtPrecio.IsEnabled = accion;
            cmbEstado.IsEnabled = accion;
        }

        //Controlar el formulario
        void ControlFormulario() 
        {
            if (dgPlatillo.Items.Count < 0)
            {
                btnNuevo.IsEnabled = true;
                btnEditar.IsEnabled = false;
                btnEliminar.IsEnabled = false;

                btnGuardar.IsEnabled = false;
                btnCancelar.IsEnabled = false;
            }
            else 
            {
                btnNuevo.IsEnabled = true;
                btnEditar.IsEnabled = true;
                btnEliminar.IsEnabled = true;

                btnGuardar.IsEnabled = false;
                btnCancelar.IsEnabled = false;
            }

            if (agregar || editar) 
            {
                btnNuevo.IsEnabled = false;
                btnEditar.IsEnabled = false;
                btnEliminar.IsEnabled = false;

                btnGuardar.IsEnabled = true;
                btnCancelar.IsEnabled = true;
            }
        }

        //Limpiar el formulario
        void LimpiarFormulario()
        {
            txtNombrePlatillo.Clear();
            txtDescripicion.Clear();
            cmbCategoria.SelectedIndex = -1;
            txtPrecio.Clear();
            cmbEstado.SelectedIndex = -1;
        }

        //Mostrar menu
        void MostrarMenu()
        {
            dgPlatillo.DataContext = PlatilloController.MostrarMenu();
        }

        //Metodos para cargar en los comboboxes
        private void CargarCategorias()
        {
            using (var conDb = new SqlConnection(conexion))
            {
                conDb.Open();
                using (var command = new SqlCommand("SELECT IdCategoria, Categoria FROM Categoria", conDb))
                {
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        cmbCategoria.Items.Add(new { Id = dr.GetInt32(0), Nombre = dr.GetString(1) });
                    }
                }
            }

            // Define qué campo mostrar
            cmbCategoria.DisplayMemberPath = "Nombre";
            cmbCategoria.SelectedValuePath = "Id";
        }

        private void CargarEstados()
        {
            using (var conDB = new SqlConnection(conexion))
            {
                conDB.Open();
                using (var command = new SqlCommand("SELECT IdEstado, Estado FROM Estado", conDB))
                {
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        cmbEstado.Items.Add(new { Id = dr.GetInt32(0), Nombre = dr.GetString(1) });
                    }
                }
            }

            //Definir en que campos mostrar
            cmbEstado.DisplayMemberPath = "Nombre";
            cmbEstado.SelectedValuePath = "Id";
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MostrarMenu();
            HabilitarFormulario(false);
            ControlFormulario();
            LimpiarFormulario();
        }

        #endregion

        #region METODOS FORMULARIO
        //Llenar formulario
        private void dgPlatillo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PlatilloModel platillo = (PlatilloModel)dgPlatillo.SelectedItem;

            if (platillo == null) 
            {
                return;
            }

            idPlatillo = platillo.PlatilloId;
            txtNombrePlatillo.Text = platillo.Platillo;
            txtDescripicion.Text = platillo.Descripcion;
            cmbCategoria.Text = platillo.Categoria;
            txtPrecio.Text = Convert.ToString(platillo.Precio);
            cmbEstado.Text = platillo.Estado;

        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            HabilitarFormulario(true);

            LimpiarFormulario();

            idEstado = 1;
            cmbEstado.IsEnabled = false;
            dgPlatillo.IsEnabled = false;
            agregar = true;
            editar = false;

            ControlFormulario();

            txtNombrePlatillo.Focus();
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            HabilitarFormulario(true);

            agregar = false;
            editar = true;

            ControlFormulario();

            txtNombrePlatillo.Focus();
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (dgPlatillo.Items.Count > 0 && !string.IsNullOrEmpty(txtNombrePlatillo.Text))
            {
                if (
                    MessageBox.Show("¿ Desear eliminar el platillo '" + idPlatillo + "' ?",
                "Accion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes
                    ) 
                {
                    idEstado = 0;
                    if (PlatilloController.EliminarPlatillo(idPlatillo, idEstado) > -1) 
                    {
                        MessageBox.Show("Registro eliminado correctamente", "Validacion",
                          MessageBoxButton.OK, MessageBoxImage.Information);

                        LimpiarFormulario();

                        MostrarMenu();

                        agregar = false;
                        editar = false;

                        ControlFormulario();
                    }
                }
            }
            else 
            {
                MessageBox.Show("Selecciona un platillo para eliminar",
                "Accion",
                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            string msj = null;

            if (ValidarFormulario())
            { 
                //Recuperamos los datos
                PlatilloModel platillo = new PlatilloModel();
                platillo.Platillo = txtNombrePlatillo.Text;
                platillo.Descripcion = txtDescripicion.Text;
                platillo.CategoriaId = (int)cmbCategoria.SelectedValue;
                platillo.Precio = Convert.ToDecimal(txtPrecio.Text);

                if (agregar)
                {
                    idEstado = 1;
                    idPlatillo = PlatilloController.AgregarPlatillo(platillo, idEstado);
                    msj = "Insercion correctamente";
                }
                else 
                {
                    platillo.EstadoId = (int)cmbEstado.SelectedValue;
                    idPlatillo = PlatilloController.EditarPlatillo(platillo, idPlatillo);
                    msj = "Actualizacion correctamente";
                }

                if (idPlatillo > 0) 
                {
                    //Limpiar formulario
                    LimpiarFormulario();

                    MessageBox.Show(msj, "Validacion del formulario",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    agregar = false;
                    editar = false;

                    HabilitarFormulario(false);

                    ControlFormulario();
                    dgPlatillo.IsEnabled = true;
                }

                MostrarMenu();
            }
        }

        private void txtNombrePlatillo_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //Validación para poder ingresar solo Texto
            e.Handled = !char.IsLetter(e.Text, 0);
        }

        private void txtPrecio_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //Validación para poder ingresar solo números
            //e.Handled = !char.IsDigit(e.Text, 0);
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (
                MessageBox.Show("Desear cancelar la operacion",
                "Accion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes
                )
            {
                LimpiarFormulario();

                HabilitarFormulario(false);

                agregar=false;
                editar=false;

                ControlFormulario();
                dgPlatillo.IsEnabled = true;

            }
        }

        #endregion
    }
}
