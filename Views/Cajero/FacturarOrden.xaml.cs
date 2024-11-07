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
        public FacturarOrden()
        {
            InitializeComponent();
           
        }

        #region METODOS PERSONALIZADOS

        void MostrarOrdenesMesa()
        {
            // Verifica si el ComboBox tiene un valor seleccionado
            if (cmbMesa.SelectedValue != null)
            {
                // Obtén el número de mesa a partir de SelectedValue
                int numeroMesa = (int)cmbMesa.SelectedValue;

                // Llama al procedimiento almacenado con el número de mesa seleccionado
                var ordenes = CajeroController.MostrarOrdenPorMesa(numeroMesa);

                // Verifica si hay órdenes y, si es así, las muestra
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
                }
                else
                {
                    MessageBox.Show("No se encontraron órdenes para esta mesa.", "Validación", MessageBoxButton.OK, MessageBoxImage.Error);
                    LimpiarFormulario();
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una mesa válida.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CargarNumeroMesa()
        {
            cmbMesa.ItemsSource = CajeroController.ObtenerMesas();
            cmbMesa.DisplayMemberPath = "Mesa"; // Mostrar el número de mesa
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
           
            txbTotal.Text = string.Empty; // Limpia el contenido del TextBlock
            dgOrdenes.ItemsSource = null; // Limpia el DataGrid
            cmbMesa.SelectedValue = null;
            txbCliente.Text = string.Empty;
            txbOrden.Text = string.Empty;
            txbUsuario.Text = string.Empty;
            txbDevolucion.Text = string.Empty;
            txtRecibido.Text = string.Empty;
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

                    // Llama al método para cambiar el estado de la orden
                    CajeroController.CambiarEstadoOrden(idOrden);


                    //Mandamos a limpiar la pestaña
                    LimpiarFormulario();
                }
            }

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CargarNumeroMesa();
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            MostrarOrdenesMesa();
        }

        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            CargarNumeroMesa();
            MessageBox.Show("Página Actualizada", "Confirmación", MessageBoxButton.OK, MessageBoxImage.Information);

        }


        #endregion

        private void txtRecibido_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }
    }
}
