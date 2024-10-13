using Rapid_Plus.Controllers.Mesero;
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //MostrarOrdenesMesa();
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
             MostrarOrdenesMesa();
        }

        #region METODOS PERSONALIZADOS
        void MostrarOrdenesMesa()
        {
            if (int.TryParse(txtMesa.Text, out int numeroMesa))
            {
                var ordenes = CajeroController.MostrarOrdenPorMesa(numeroMesa);

                // Verifica si hay órdenes y, si es así, muestra el primer registro
                if (ordenes.Count > 0)
                {
                    
                    txtCliente.Text = ordenes[0].NombreCliente; 
                    txtUsuario.Text = ordenes[0].NombreUsuario; 
                    dtpFechaNacimiento.SelectedDate = ordenes[0].FechaOrden;
                    dgOrdenes.ItemsSource = ordenes;

                    // Calcular el total
                    decimal total = ordenes.Sum(o => o.Subtotal);
                    txbTotal.Text = $"${total:F2}"; // Actualiza el TextBlock con el total
                }
                else
                {
                    MessageBox.Show("No se encontraron órdenes para esta mesa.", "Validación", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtCliente.Text = "";
                    txtUsuario.Text = "";
                    dtpFechaNacimiento.SelectedDate = null; // Limpia el DatePicker
                    dgOrdenes.ItemsSource = null; // Limpia el DataGrid
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un número de mesa válido.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
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


        #endregion


    }
}
