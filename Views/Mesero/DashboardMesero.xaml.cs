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

namespace Rapid_Plus.Views.Mesero
{
    /// <summary>
    /// Lógica de interacción para DashboardMesero.xaml
    /// </summary>
    public partial class DashboardMesero : Window
    {

        #region Instancia de las páginas
        TomarOrden tomarOrden = new TomarOrden();
        VerOrden verOrden = new VerOrden();
        CrearOrden crearOrden = new CrearOrden();
        #endregion
        public DashboardMesero(int usuarioID)
        {
            InitializeComponent();
        }

        #region NAVEGACIÓN HACIA LAS PÁGINAS
        //Abrir páginas
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
        #endregion

        #region ESTILOS
        private void BotonMesero_MouseEnter(object sender, MouseEventArgs e)
        {
            Button boton = sender as Button;

            if (boton != null)
            {
                boton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0B5563"));
            }
        }

        private void BotonMesero_MouseLeave(object sender, MouseEventArgs e)
        {
            Button boton = sender as Button;

            if (boton != null)
            {
                boton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5299D3"));
            }
        }
        #endregion

        #region EVENTOS
        //Cerrar Sesión
        private void btnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show("Desea Cerrar Sesión?", "Cerrar sesión", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                MainWindow login = new MainWindow();
                login.Show();
                this.Close();
            }

        }

        //Cerrar Ventana
        private void BtnCerrarVentana_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            frContent.NavigationService.Navigate(verOrden);
        }


        #endregion


    }
}
