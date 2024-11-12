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
using Rapid_Plus.Controllers;
using Rapid_Plus.Models;
using Rapid_Plus.Models.Mesero;

namespace Rapid_Plus.Views.Cajero
{
    /// <summary>
    /// Lógica de interacción para FacturarOrden.xaml
    /// </summary>
    public partial class FacturarOrden : Page
    {
        private DispatcherTimer timer;

        public FacturarOrden()
        {
            InitializeComponent();
            IniciarTemporizador();
            ActualizarEstadoControles(false);
        }

        #region METODOS PERSONALIZADOS

        private void IniciarTemporizador()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Timer_Tik;
            timer.Start();
        }

        private void Timer_Tik(object sender, EventArgs e)
        {
            CargarNumeroMesa();
        }

        void MostrarOrdenesMesa()
        {
            // Verifica si el ComboBox tiene un valor seleccionado
            if (cmbMesa.SelectedValue != null)
            {
                // Obtenemos el número de mesa a partir de SelectedValue
                int numeroMesa = (int)cmbMesa.SelectedValue;

                // Llama al procedimiento almacenado con el número de mesa seleccionado
                var ordenes = CajeroController.MostrarOrdenPorMesa(numeroMesa);

               //Verifica si hay ordenes
                if (ordenes.Count > 0)
                {
                    dgOrdenes.ItemsSource = ordenes;

                    //Mostramos la info en los TexBlock
                    txbCliente.Text = ordenes.First().NombreCliente;
                    txbOrden.Text = ordenes.First().IdOrden.ToString();
                    txbUsuario.Text = ordenes.First().NombreUsuario;

                    // Calcular el total
                    decimal total = ordenes.Sum(o => o.Subtotal);
                    txbTotal.Text = $"${total:F2}"; // Actualiza el TextBlock con el total
                    ActualizarEstadoControles(true);
                }
                else
                {
                    MessageBox.Show("No se encontraron órdenes para esta mesa.", "Validación", MessageBoxButton.OK, MessageBoxImage.Error);
                    LimpiarFormulario();
                }
            }
           
        }

        private void CargarNumeroMesa()
        {
            cmbMesa.ItemsSource = CajeroController.ObtenerMesas();
            cmbMesa.DisplayMemberPath = "Mesa"; 
            cmbMesa.SelectedValuePath = "Mesa";
        }

        #endregion

        #region EVENTOS BOTONES/ACCIONES
        private void txtRecibido_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Intenta convertir el texto del TextBox a decimal
            if (decimal.TryParse(txtRecibido.Text, out decimal dineroRecibido))
            {
                // Obtén el total ya calculado
                if (decimal.TryParse(txbTotal.Text.Replace("$", "").Trim(), out decimal total))
                {
                    // Calcula el vuelto
                    decimal vuelto = dineroRecibido - total;

                    // Verifica si el vuelto es mayor o igual a 0
                    if (vuelto >= 0)
                    {
                        // Actualiza el TextBlock con el vuelto
                        txbDevolucion.Text = $": ${vuelto:F2}";
                    }
                    else
                    {
                        // Mensaje cuando la cantidad recibida es insuficiente
                        txbDevolucion.Text = "Cantidad insuficiente";
                    }
                }
                else
                {
                    // Si no se puede convertir el total, muestra un mensaje o restablece el vuelto
                    txbDevolucion.Text = "Error en total.";
                }
            }
            else
            {
                // Si el valor no es válido, restablece el vuelto
                txbDevolucion.Text = ": $0.00";
            }
        }

        //Metodo para limpiar el formulario
        void LimpiarFormulario()
        {

            txbTotal.Text = string.Empty; 
            dgOrdenes.ItemsSource = null; 
            cmbMesa.SelectedValue = null;
            txbCliente.Text = string.Empty;
            txbOrden.Text = string.Empty;
            txbUsuario.Text = string.Empty;
            txbDevolucion.Text = string.Empty;
            txtRecibido.Text = string.Empty;
            ActualizarEstadoControles(false);
        }

        //Metodo para controlar los botones
        private void ActualizarEstadoControles(bool habilitar)
        {
            btnRealizar.IsEnabled = habilitar;
            btnCancelar.IsEnabled = habilitar;
            txtRecibido.IsEnabled = habilitar;
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            //Validamos si quiere confirmar la cancelacion
            if (MessageBox.Show("Desear cancelar la operacion","Accion",MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                LimpiarFormulario();
            }
        }

        //Boton para realizar la facturación
        private void btnRealizar_Click_1(object sender, RoutedEventArgs e)
        {
            decimal recibidoPago;

            if(string.IsNullOrEmpty(txtRecibido.Text))
            {
                recibidoPago = 0;
            }
            else
            {
                recibidoPago = Convert.ToDecimal(txtRecibido.Text); 
            }

            if(recibidoPago < 0){
                MessageBox.Show("Ingrese cantidad positiva", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            
            

            // Verifica que el DataGrid tenga al menos un elemento
            if (dgOrdenes.Items.Count > 0 && dgOrdenes.Items[0] is OrdenesModel primeraOrden)
            {
                // Muestra el mensaje de confirmación
                MessageBoxResult resultado = MessageBox.Show(
                    "¿Estás seguro de Facturar la Orden?",
                    "Confirmación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                // Si el usuario confirma, cambia el estado de la orden
                if (resultado == MessageBoxResult.Yes)
                {
                    int idOrden = primeraOrden.IdOrden; // Obtén el IdOrden de la primera fila

                   //Llamamos el metodo para cambiar el metodo
                    CajeroController.CambiarEstadoOrden(idOrden);


                    //Mandamos a limpiar la pestaña
                    LimpiarFormulario();
                    MostrarOrdenesMesa();
                }
            }

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LimpiarFormulario();
            CargarNumeroMesa();
            
        }

        private void cmbMesa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MostrarOrdenesMesa();
        }

        private void cmbMesa_DropDownOpened(object sender, EventArgs e)
        {
            if (cmbMesa.Items.Count == 0)
            {
                MessageBox.Show("No hay mesas con ordenes listas para facturar", "Mesas", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        #endregion
    }
}
