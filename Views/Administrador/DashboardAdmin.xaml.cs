using Rapid_Plus.Reports;
using Rapid_Plus.Views.Cajero;
using Rapid_Plus.Views.JefeDeCocina;
using Rapid_Plus.Views.Mesero;
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
using System.Windows.Shapes;

namespace Rapid_Plus.Views.Administrador
{
    /// <summary>
    /// Lógica de interacción para DashboardAdmin.xaml
    /// </summary>
    public partial class DashboardAdmin : Window
    {

        #region INSTANCIAS DE LAS PAGINAS
        Contactos contacto = new Contactos();
        Menu menu = new Menu();
        Configuraciones configuracion = new Configuraciones();
        ParametroReporte prReport = new ParametroReporte();
        TomarOrden tomarOrden = new TomarOrden();
        VerOrden verOrden = new VerOrden();
        private CrearOrden crearOrden;
        GestionClientes clientes = new GestionClientes();
        VerOrdenesT verOrdenes = new VerOrdenesT();
        FacturarOrden factura = new FacturarOrden();
        EstadoOrden estadoOrden = new EstadoOrden();

        #endregion

        public DashboardAdmin(int usuarioId)
        {
            InitializeComponent();
            crearOrden = new CrearOrden(usuarioId);
        }

        #region METODOS ESTILOS

        private void BotonAdmin_MouseEnter(object sender, MouseEventArgs e)
        {
            Button boton = sender as Button;

            if (boton != null)
            {
                boton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0B5563"));
            }
        }

        private void BotonAdmin_MouseLeave(object sender, MouseEventArgs e)
        {
            Button boton = sender as Button;

            if (boton != null)
            {
                boton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5299D3"));
            }
        }

        #endregion

        #region METODOS FORMULARIO

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            frContent.NavigationService.Navigate(contacto);
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            frContent.NavigationService.Navigate(menu);
        }

        private void btnUsuario_Click(object sender, RoutedEventArgs e)
        {
            frContent.NavigationService.Navigate(contacto);
        }

        private void btnMas_Click(object sender, RoutedEventArgs e)
        {
            frContent.NavigationService.Navigate(configuracion);
        }

        private void btnOrdenesFinal_Click(object sender, RoutedEventArgs e)
        {
            frContent.NavigationService.Navigate(verOrdenes);
        }

        private void btnReportes_Click(object sender, RoutedEventArgs e)
        {
            var prReport = new ParametroReporte();
            prReport.ShowDialog();
        }

        private void btnTomarOrden_Click(object sender, RoutedEventArgs e)
        {
            frContent.NavigationService.Navigate(crearOrden);
        }

        private void btnVerOrden_Click(object sender, RoutedEventArgs e)
        {

            frContent.NavigationService.Navigate(verOrden);
        }

        private void btnGestionar_Click(object sender, RoutedEventArgs e)
        {
            frContent.NavigationService.Navigate(tomarOrden);
        }

        private void btnClientes_Click(object sender, RoutedEventArgs e)
        {
            frContent.NavigationService.Navigate(clientes);
        }

        private void btnOrdenes_Click(object sender, RoutedEventArgs e)
        {
            frContent.NavigationService.Navigate(estadoOrden);
        }

        private void btnFacturar_Click(object sender, RoutedEventArgs e)
        {
            frContent.NavigationService.Navigate(factura);
        }

        private void btnCerrarVentana_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            if (
                MessageBox.Show("Desea Cerrar Sesión?",
                "Cerrar sesión",
                MessageBoxButton.YesNo,
                MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                MainWindow login = new MainWindow();
                login.Show();
                this.Close();
            }
        }

        #endregion

       
    }
}
