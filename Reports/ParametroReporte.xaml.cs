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
using System.Windows.Shapes;

namespace Rapid_Plus.Reports
{
    /// <summary>
    /// Lógica de interacción para ParametroReporte.xaml
    /// </summary>
    public partial class ParametroReporte : Window
    {
        public ParametroReporte()
        {
            InitializeComponent();
        }

        #region VARIABLES LOCALES
        ReporteFechas rpFecha = new ReporteFechas();
        ReporteCategoria rpCategoria = new ReporteCategoria();

        private void btnReporteFecha_Click(object sender, RoutedEventArgs e)
        {
            var rpFecha = new ReporteFechas();
            rpFecha.ShowDialog();
        }
        private void btnReporteCategoria_Click(object sender, RoutedEventArgs e)
        {
            var rpCategoria = new ReporteCategoria();
            rpCategoria.ShowDialog();
        }

        #endregion

    }
}
