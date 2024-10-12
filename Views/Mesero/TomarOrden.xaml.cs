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
    /// Lógica de interacción para TomarOrden.xaml
    /// </summary>
    public partial class TomarOrden : Page
    {
        public TomarOrden()
        {
            InitializeComponent();
            CargarNumeroMesa();

        }

        int numeroMesa = -1;
        int idOrden = -1;
        int idplatillo = -1;
        private void CargarNumeroMesa()
        {
            using (var conDb = new SqlConnection(Properties.Settings.Default.DbRapidPlus))
            {
                conDb.Open();
                using (var command = new SqlCommand("SELECT Mesa FROM Mesas WHERE Id_Estado = 0", conDb)) //Muestra unicamente las disponibles
                {
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        cmbMesa.Items.Add(dr.GetInt32(0));
                    }
                }
            }

            // Define qué campo mostrar
            cmbMesa.SelectedValuePath = "Id";
        }
        private void LimpiarObjetos()
        {
            cmbMesa.SelectedIndex = -1;
            txtCantidad.Clear();
            txbEstado.Text = null;
            txbOrden.Text = null;
            txbPlatillo.Text = null;
        }

        private int NumeroMesa()
        {
            
            if (cmbMesa.SelectedIndex != -1)
            {
                // Asume que el valor seleccionado es un entero
                numeroMesa = (int)cmbMesa.SelectedItem;

            }
            else
            {
                numeroMesa = -1;
            }
            return numeroMesa;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LimpiarObjetos();
            MostrarPlatillos();
        }

        private void cmbMesa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            numeroMesa = NumeroMesa();

            // Carga las órdenes para la mesa seleccionada
            var ordenes = MeseroController.ListarOrdenesPorMesa(numeroMesa);
            
            dgOrdenes.DataContext = ordenes; // Asigna la lista al DataContext

            // Verifica si hay órdenes y muestra el IdOrden de la primera fila, si existe
            if (ordenes.Count > 0)
            {
                idOrden = ordenes[0].IdOrden; // Muestra el IdOrden de la primera
                txbOrden.Text = idOrden.ToString();
                txbEstado.Text = ordenes[0].EstadoOrden.ToString();
            }
            else
            {
                txbOrden.Text = "0"; 
            }


        }
        private void MostrarPlatillos()
        {
            dgPlatillos.DataContext = MeseroController.ListaPlatillos();
        }
        private bool ValidarFomrulario()
        {
            bool estado = true;
            string mensaje = null;

            if (string.IsNullOrEmpty(txbOrden.Text))
            {
                estado = false;
                mensaje += "Número de orden\n";
            }
            if (idplatillo == -1)
            {
                estado = false;
                mensaje += "Platillo\n";
            }
            if (string.IsNullOrEmpty(txtCantidad.Text))
            {
                estado = false;
                mensaje += "Cantidad";
            }

            if (!estado)
            {
                MessageBox.Show("Debe completar los campos:\n" + mensaje, "Validación de formulario", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return estado;
        }

        private void dgPlatillos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OrdenesModel ordenes = (OrdenesModel)dgPlatillos.SelectedItem;
            if (ordenes == null)
            {
                return;
            }
            txbPlatillo.Text = ordenes.NombrePlatillo;
            idplatillo = ordenes.IdPlatillo;

        } 
        private void btnAgregarOrden_Click(object sender, RoutedEventArgs e)
        {
            string mensaje = null;
            if (ValidarFomrulario())
            {
                OrdenesModel orden = new OrdenesModel();
                orden.IdOrden = Convert.ToInt32(txbOrden.Text);
                orden.Cantidad = Convert.ToInt32(txtCantidad.Text);
                orden.IdPlatillo = idplatillo;

                idOrden = MeseroController.InsertarOrden(orden);
                mensaje = "Orden creada con éxito";

                if (idOrden > 0)
                {
                    LimpiarObjetos();
                    MessageBox.Show(mensaje, "Validación de formulario", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
        }

        

    }
}
