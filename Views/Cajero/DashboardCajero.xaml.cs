using Rapid_Plus.Views.Administrador;
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

namespace Rapid_Plus.Views.Cajero
{
    /// <summary>
    /// Lógica de interacción para DashboardCajero.xaml
    /// </summary>
    public partial class DashboardCajero : Window
    {
        public DashboardCajero(int usuarioId)
        {
            InitializeComponent();
            crearOrden = new CrearOrden(usuarioId);
        }

        
        #region Instancias de las paginas
        FacturarOrden factura = new FacturarOrden();
        TomarOrden tomarOrden = new TomarOrden();
        VerOrden verOrden = new VerOrden();
        private CrearOrden crearOrden;
        GestionClientes clientes = new GestionClientes();

        #endregion


        #region Estilos de Metodos
        private void btnCerrarVentana_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnFacturar_MouseEnter(object sender, MouseEventArgs e)
        {
            Button boton = sender as Button;

            if (boton != null)
            {
                boton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0B5563"));
            }
        }

        private void btnFacturar_MouseLeave(object sender, MouseEventArgs e)
        {
            btnFacturar.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5299D3"));
        }

        private void btnCerrarSesion_MouseEnter(object sender, MouseEventArgs e)
        {
            Button boton = sender as Button;

            if (boton != null)
            {
                boton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0B5563"));
            }
        }

        private void btnCerrarSesion_MouseLeave(object sender, MouseEventArgs e)
        {
            btnCerrarSesion.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5299D3"));
        }

        private void btnClientes_MouseEnter(object sender, MouseEventArgs e)
        {
            Button boton = sender as Button;

            if (boton != null)
            {
                boton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0B5563"));
            }
        }

        private void btnClientes_MouseLeave(object sender, MouseEventArgs e)
        {
            Button boton = sender as Button;

            if (boton != null)
            {
                boton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5299D3"));
            }
        }

        private void btnCrearOrden_MouseEnter(object sender, MouseEventArgs e)
        {
            Button boton = sender as Button;

            if (boton != null)
            {
                boton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0B5563"));
            }
        }

        private void btnCrearOrden_MouseLeave(object sender, MouseEventArgs e)
        {
            Button boton = sender as Button;

            if (boton != null)
            {
                boton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5299D3"));
            }
        }

        private void btnVerOrden_MouseEnter(object sender, MouseEventArgs e)
        {
            Button boton = sender as Button;

            if (boton != null)
            {
                boton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0B5563"));
            }
        }

        private void btnVerOrden_MouseLeave(object sender, MouseEventArgs e)
        {
            Button boton = sender as Button;

            if (boton != null)
            {
                boton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5299D3"));
            }
        }

        private void btnGestionar_MouseEnter(object sender, MouseEventArgs e)
        {
            Button boton = sender as Button;

            if (boton != null)
            {
                boton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0B5563"));
            }
        }

        private void btnGestionar_MouseLeave(object sender, MouseEventArgs e)
        {
            Button boton = sender as Button;

            if (boton != null)
            {
                boton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5299D3"));
            }
        }
        #endregion


        #region Navegación Hacia las Paginas
        private void btnFacturar_Click(object sender, RoutedEventArgs e)
        {
            frContent.NavigationService.Navigate(factura);
        }

        private void btnClientes_Click(object sender, RoutedEventArgs e)
        {
            frContent.NavigationService.Navigate(clientes);
        }

        private void btnCrearOrden_Click(object sender, RoutedEventArgs e)
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

        //Colocamos la factura para inicializar al entrar a Cajero
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            frContent.NavigationService.Navigate(factura);
        }

        //Metodo para cerrar sesión
        private void btnCerrarSesion_Click_1(object sender, RoutedEventArgs e)
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
