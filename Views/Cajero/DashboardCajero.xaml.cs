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
        public DashboardCajero()
        {
            InitializeComponent();
        }
        #region Instancias de las paginas
        FacturarOrden factura = new FacturarOrden();
     
        #endregion


        #region Estilos de Metodos
        private void btnCerrarVentana_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

       

        private void btnFacturar_MouseEnter(object sender, MouseEventArgs e)
        {
            btnFacturar.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD63C3C"));
        }

        private void btnFacturar_MouseLeave(object sender, MouseEventArgs e)
        {
            btnFacturar.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF008592"));
        }

        private void btnCerrarSesion_MouseEnter(object sender, MouseEventArgs e)
        {
            btnCerrarSesion.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD63C3C"));
        }

        private void btnCerrarSesion_MouseLeave(object sender, MouseEventArgs e)
        {
            btnCerrarSesion.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF008592"));
        }
        #endregion

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

        private void btnFacturar_Click(object sender, RoutedEventArgs e)
        {
            frContent.NavigationService.Navigate(factura);
        }
    }
}
