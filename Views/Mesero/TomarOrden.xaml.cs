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
            MostrarOrdenes();
            MostrarPlatillos();
        }
        void MostrarOrdenes()
        {
            dgOrdenes.DataContext = MeseroController.ListarOrdenes();
        }
        void MostrarPlatillos()
        {
            dgPlatillos.DataContext = MeseroController.ListaPlatillos();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
           //txtCantidad.Clear();
        }
    }
}
