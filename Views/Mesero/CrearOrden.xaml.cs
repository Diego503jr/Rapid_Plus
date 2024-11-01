using Rapid_Plus.Controllers.Mesero;
using Rapid_Plus.Models;
using Rapid_Plus.Models.Mesero;
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

namespace Rapid_Plus.Views.Mesero
{
    /// <summary>
    /// Lógica de interacción para CrearOrden.xaml
    /// </summary>
    public partial class CrearOrden : Page
    {
        public CrearOrden()
        {
            InitializeComponent();
        }
        #region Declaracion de variables locales
        private DateTime fecha = DateTime.Now;
        private int idOrden = 0;
        private int idCliente = -1;
        #endregion


        #region MÉTODOS PERSONALIZADOS
        //Llenar combobox de numero de mesa
        private int CargarNumeroMesa()
        {
            int numMesas = -1;
            using (var conDb = new SqlConnection(Properties.Settings.Default.DbRapidPlus))
            {
                conDb.Open();
                using (var command = new SqlCommand("SELECT IdMesa, Mesa FROM Mesa WHERE IdEstado = 1", conDb)) 
                {
                    SqlDataReader dr = command.ExecuteReader();
                    var mesas = new List<dynamic>();
                    while (dr.Read())
                    {
                        mesas.Add(new { Id = dr.GetInt32(0), Mesa = dr.GetInt32(1) }); 
                    }

                    cmbMesa.ItemsSource = mesas;
                    numMesas = mesas.Count;
                }
            }
            cmbMesa.DisplayMemberPath = "Mesa";  
            cmbMesa.SelectedValuePath = "Id";

            return numMesas ;
        }

        private bool ValidarFomrulario()
        {
            bool estado = true;
            string mensaje = null;

            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                estado = false;
                mensaje += "Nombre de cliente\n";
            }
            if (string.IsNullOrEmpty(txtApellido.Text))
            {
                estado = false;
                mensaje += "Apellido de cliente\n";
            }
            if (string.IsNullOrEmpty(cmbMesa.Text))
            {
                estado = false;
                mensaje += "Número de mesa";
            }

            if (!estado)
            {
                MessageBox.Show("Debe completar los campos:\n" + mensaje, "Validación de formulario", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return estado;
        }

        private void LimpiarObjetos()
        {
            cmbMesa.SelectedIndex = -1;
            txtApellido.Clear();
            txtNombre.Clear();
            dgClientes.SelectedIndex = -1;
        }

        private void MostrarClientes()
        {
            dgClientes.DataContext = MeseroController.ListarClientes();
        }

        #endregion


        #region EVENTOS
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LimpiarObjetos();
            CargarNumeroMesa();
            MostrarClientes();
        }

        private void btnCrearOrden_Click(object sender, RoutedEventArgs e)
        {
            string mensaje = null;
            if (ValidarFomrulario())
            {
                OrdenesModel orden = new OrdenesModel();
                orden.NombreCliente = txtNombre.Text;
                orden.ApellidoCliente = txtApellido.Text;
                orden.UsuarioId = 0;
                orden.FechaOrden = fecha;
                orden.Total = 0;
                orden.Mesa = (int)cmbMesa.SelectedValue;
                orden.IdEstadoOrden = 0;

                idOrden = MeseroController.CrearOrden(orden);
                mensaje = "Orden creada con éxito";

                if (idOrden > 0)
                {
                    LimpiarObjetos();
                    MessageBox.Show(mensaje, "Validación de formulario", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }

        }

        private void dgClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OrdenesModel ordenes = (OrdenesModel)dgClientes.SelectedItem;
            if (ordenes == null)
            {
                return;
            }
            txtNombre.Text = ordenes.NombreCliente;
            txtApellido.Text = ordenes.ApellidoCliente;
            idCliente = ordenes.IdCliente;
        }

        private void cmbMesa_DropDownOpened(object sender, EventArgs e)
        {
            if (CargarNumeroMesa() <= 0)
            {
                MessageBox.Show("No hay mesas disponibles", "Mesas", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarObjetos();
        }


        #endregion


    }
}
