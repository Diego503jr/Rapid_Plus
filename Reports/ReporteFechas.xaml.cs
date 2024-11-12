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

namespace Rapid_Plus.Reports
{
    /// <summary>
    /// Lógica de interacción para ReporteFechas.xaml
    /// </summary>
    public partial class ReporteFechas : Window
    {
        public ReporteFechas()
        {
            InitializeComponent();
        }

        void LimpiarFormulario()
        {
            dtpInicio.SelectedDate = null;
            dtpFin.SelectedDate = null;
        }

        //Parametros para abrir el reporte
        private void btnGnRFecha_Click(object sender, RoutedEventArgs e)
        {
            //Asignar valores se ingresen o no
            DateTime? fechaInicio = dtpInicio.SelectedDate;
            DateTime? fechaFin = dtpFin.SelectedDate;

            rptFechas rpt = new rptFechas();
            vwRapidPlus vw = new vwRapidPlus();

            // Cargar el reporte
            rpt.Load(@"rptFechas.rpt");

            // Verificar fechas y asignar parámetros
            if (fechaInicio.HasValue && fechaFin.HasValue)
            {
                rpt.SetParameterValue("@FechaInicio", fechaInicio.Value);
                rpt.SetParameterValue("@FechaFin", fechaFin.Value);
            }
            else
            {
                // Si no hay valor, establece DBNull
                rpt.SetParameterValue("@FechaInicio", DBNull.Value);
                rpt.SetParameterValue("@FechaFin", DBNull.Value);
            }

            // Asignar el reporte al visor
            vw.crvReportRapidPlus.ViewerCore.ReportSource = rpt;
            vw.ShowDialog();

            //Limpiamos los campos
            LimpiarFormulario();
        }
    }
}
