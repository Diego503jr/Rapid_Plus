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
    /// Lógica de interacción para VerOrden.xaml
    /// </summary>
    public partial class VerOrden : Page
    {
        public VerOrden()
        {
            InitializeComponent();
            MostrarOrdenes();
        }

        void MostrarOrdenes()
        {
            dgOrdenes.DataContext = MeseroController.ListarOrdenes();
        }

       
    }
}
