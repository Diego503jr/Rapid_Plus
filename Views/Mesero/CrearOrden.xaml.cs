using Rapid_Plus.Controllers;
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
using System.Windows.Threading;

namespace Rapid_Plus.Views.Mesero
{
    /// <summary>
    /// Lógica de interacción para CrearOrden.xaml
    /// </summary>
    public partial class CrearOrden : Page
    {
        
        public CrearOrden(int usuarioID)
        {
            InitializeComponent();
            //Usuario logueado para crear orden
            usuarioId = usuarioID;
            IniciarTemporizador();
        }

        #region Declaracion de variables locales
        private int idOrden = 0;
        private int idCliente = -1;
        private int usuarioId;
        private bool agregando = false;
        private DispatcherTimer timer;
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

                    //Cuenta la cantidad de elementos (mesas) encontrados 
                    cmbMesa.ItemsSource = mesas;
                    numMesas = mesas.Count;
                }
            }

            //Define que campos mostrar
            cmbMesa.DisplayMemberPath = "Mesa";  
            cmbMesa.SelectedValuePath = "Id";

            return numMesas ;
        }

        //Validar campos llenos
        private bool ValidarFormulario()
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

        //Limpiar objetos del formulario
        private void LimpiarObjetos()
        {
            cmbMesa.SelectedIndex = -1;
            txtApellido.Clear();
            txtNombre.Clear();
            dgClientes.SelectedIndex = -1;
        }

        //Listar clientes en datagrid
        private void MostrarClientes()
        {
            dgClientes.DataContext = ClienteController.MostrarClientes();
        }

        //Activar/Desactivar botones
        private void ControlAcciones(bool agregando)
        {
            btnGuardar.IsEnabled = agregando;
            btnCancelar.IsEnabled = agregando;
            txtNombre.IsEnabled = false;
            txtApellido.IsEnabled = false;
            btnCrear.IsEnabled = !agregando;
            
            dgClientes.IsEnabled = agregando;
            cmbMesa.IsEnabled = agregando;
           
        }

        //Temporizador para refrescar
        private void IniciarTemporizador()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += Timer_Tik;
            timer.Start();
        }

        #endregion

        #region EVENTOS

        //Cargas por 'defecto'
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            agregando = false;
            ControlAcciones(agregando);
            LimpiarObjetos();
            CargarNumeroMesa();
            MostrarClientes();
            
        }
        //Validar que solo se ingrese texto en los combobox
        private void txtFiltro_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //Validación para poder ingresar solo Texto
            e.Handled = !char.IsLetter(e.Text, 0);
        }

        //Obtiene datos de registro en datagrid
        private void dgClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClienteModel clientes = (ClienteModel)dgClientes.SelectedItem;
            if (clientes == null)
            {
                return;
            }
            txtNombre.Text = clientes.NombreCliente;
            txtApellido.Text = clientes.ApellidoCliente;
            idCliente = clientes.IdCliente;
        }

        //Evento se activa al desplegar el combobox, verifica si no existen elementos
        private void cmbMesa_DropDownOpened(object sender, EventArgs e)
        {
            //Si no existen elementos en el combobox
            if (CargarNumeroMesa() <= 0)
            {
                MessageBox.Show("No hay mesas disponibles", "Mesas", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
       
        //Búsqueda de cliente (Filtro)
        private void txtFiltro_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dgClientes.ItemsSource).Filter = item =>
            {
                var texto = item as dynamic;
                if (texto == null) return false;

                string filtro = txtFiltro.Text.ToLower();
                var textoFiltro = filtro.Split(' '); //Separa la cadena escrita

                string nombreCompleto = (texto.NombreCliente + " " + texto.ApellidoCliente).ToLower();

                return textoFiltro.All(palabra => nombreCompleto.Contains(palabra));
            };

            CollectionViewSource.GetDefaultView(dgClientes.ItemsSource).Refresh();
        }

        //Refrescar página
        private void Timer_Tik(object sender, EventArgs e)
        {
            MostrarClientes();
        }

        //BOTONES
        #region BOTONES
        //Acciones con botones
        //Crear Orden
        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            agregando = true;
            LimpiarObjetos();
            ControlAcciones(agregando);
        }

        //Guardar la creación de la orden
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            string mensaje = null;
            if (ValidarFormulario())
            {
                OrdenesModel orden = new OrdenesModel();
                DateTime fecha = DateTime.Now; //Fecha y hora actual
                orden.IdCliente = idCliente;
                orden.IdUsuario = usuarioId;
                orden.FechaOrden = fecha;
                orden.Total = 0;
                orden.Mesa = (int)cmbMesa.SelectedValue;
                orden.IdEstadoOrden = 0;

                idOrden = OrdenController.CrearOrden(orden);
                mensaje = "Orden creada con éxito";

                if (idOrden > 0)
                {
                    LimpiarObjetos();
                    agregando = false;
                    ControlAcciones(agregando);
                    MessageBox.Show(mensaje, "Validación de formulario", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
        }

        //Cancela la operación
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Desea cancelar la operación", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                LimpiarObjetos();
                agregando = false;
                ControlAcciones(agregando);
            }
        }
        #endregion

        #endregion


    }
}
