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

namespace Rapid_Plus.Views.JefeDeCocina
{
    /// <summary>
    /// Lógica de interacción para DashboardJefeCocina.xaml
    /// </summary>
    public partial class DashboardJefeCocina : Window
    {
        #region Instancia de las páginas
        EstadoOrden estadoOrden = new EstadoOrden();
        #endregion
        public DashboardJefeCocina()
        {
            InitializeComponent();
        }

        private void btnOrdenes_Click(object sender, RoutedEventArgs e)
        {
            frContent.NavigationService.Navigate(estadoOrden);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            frContent.NavigationService.Navigate(estadoOrden);
        }

        private void btnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Desea Cerrar Sesión?", "Cerrar sesión", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                MainWindow login = new MainWindow();
                login.Show();
                this.Close();
            }
        }

        private void btnCerrarVentana_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        
    }
}
