using Rapid_Plus.Controllers;
using Rapid_Plus.Models;
using Rapid_Plus.Models.Mesero;
using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para GestionClientes.xaml
    /// </summary>
    public partial class GestionClientes : Page
    {
        public GestionClientes()
        {
            InitializeComponent();
        }

        #region DECLARACIÓN DE VARIABLES LOCALES
        private int idCliente = -1;
        private bool agregando = false, editando = false;
        #endregion

        #region MÉTODOS PERSONALIZADOS

        //Limpia cajas de texto y seleccion de datagrid
        private void LimpiarObjetos()
        {
            txtApellido.Clear();
            txtNombre.Clear();
            dgClientes.SelectedIndex = -1;
        }

        //Validacion de campos completos
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

            if (!estado)
            {
                MessageBox.Show("Debe completar los campos:\n" + mensaje, "Validación de formulario", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return estado;
        }

        //Muestra clientes en el datagrid
        private void MostrarClientes()
        {
            dgClientes.DataContext = ClienteController.MostrarClientes();
        }
        
        //Activa botones y campos
        private void ControlAcciones()
        {
            bool accion = editando || agregando;

            btnCrear.IsEnabled = !accion;
            btnEditar.IsEnabled = !accion;
            btnGuardar.IsEnabled = accion;
            btnCancelar.IsEnabled = accion;
            txtNombre.IsEnabled = accion;
            txtApellido.IsEnabled = accion;
            dgClientes.IsEnabled = editando;
        }

        #endregion

        #region EVENTOS

        //Busqueda de cliente
        private void txtFiltro_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dgClientes.ItemsSource).Filter = item =>
            {
                var objeto = item as dynamic;
                if (objeto == null) return false;

                string filtro = txtFiltro.Text.ToLower();
                var palabrasFiltro = filtro.Split(' ');

                string nombreCompleto = (objeto.NombreCliente + " " + objeto.ApellidoCliente).ToLower();

                return palabrasFiltro.All(palabra => nombreCompleto.Contains(palabra));
            };

            CollectionViewSource.GetDefaultView(dgClientes.ItemsSource).Refresh();
        }

        //Selección de registro en datagrid
        private void dgClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClienteModel cliente = (ClienteModel)dgClientes.SelectedItem;
            if (cliente == null)
            {
                return;
            }
            txtNombre.Text = cliente.NombreCliente;
            txtApellido.Text = cliente.ApellidoCliente;
            idCliente = cliente.IdCliente;
        }

        //Cargas por 'defecto'
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LimpiarObjetos();
            MostrarClientes();
            agregando = false;
            editando = false;
            ControlAcciones();
        }

        //Acciones con botones
        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            //Limpia campos
            txtNombre.Clear();
            txtApellido.Clear();

            //Agregando
            agregando = true;
            editando = false;

            //Método para activar y desactivar campos y botones
            ControlAcciones();
        }
        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {

            //Editand
            agregando = false;
            editando = true;

            //Método para activar y desactivar campos y botones
            ControlAcciones();
        }
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            string mensaje = null;
            if (ValidarFomrulario())
            {

                //Obtiene datos del cliente
                ClienteModel cliente = new ClienteModel();
                cliente.NombreCliente = txtNombre.Text;
                cliente.ApellidoCliente = txtApellido.Text;


                //Verifica si se está editando o agregando registro y llamada al método correspondiente
                if (agregando)
                {
                    idCliente = ClienteController.CrearCliente(cliente);
                    mensaje = "Cliente agregado con éxito";
                    MostrarClientes();
                }
                else
                {
                    idCliente = ClienteController.ActualizarCliente(cliente, idCliente);
                    mensaje = "Cliente editado con éxito";
                    MostrarClientes();
                }

                //Verifica si los datos se han podido ingresar con éxito
                if (idCliente > 0)
                {
                    LimpiarObjetos();
                    MessageBox.Show(mensaje, "Validación de formulario", MessageBoxButton.OK, MessageBoxImage.Information);

                    agregando = false;
                    editando = false;
                    ControlAcciones();
                }
                else
                {
                    LimpiarObjetos();
                    agregando = false;
                    editando = false;
                    ControlAcciones();
                }

            }
        }
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Desea cancelar la operación", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                LimpiarObjetos();

                agregando = false;
                editando = false;
                ControlAcciones();
            }
        }
        #endregion
    }
}
