using System;
using System.Collections.Generic;
using System.Data.Common;
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
    /// Lógica de interacción para ParametrosReporte.xaml
    /// </summary>
    public partial class ParametrosReporte : Window
    {
        public ParametrosReporte()
        {
            InitializeComponent();
            CargarCategoria();
        }

        #region VARIABLES LOCALES

        //Conexion a la DB
        SqlConnection conDBRP = new SqlConnection(Properties.Settings.Default.DbRapidPlus);

        //Limpiar campos
        void LimpiarFormulario()
        {
            dtpInicio.SelectedDate = null;
            dtpFin.SelectedDate = null;
            cmbCategoria.SelectedIndex = -1;
        }

        //Cargar datos y agregarlos al cmb
        private void CargarCategoria()
        {
            using (var conDb = new SqlConnection(Properties.Settings.Default.DbRapidPlus))
            {
                conDb.Open();
                using (var command = new SqlCommand("SELECT IdCategoria , Categoria FROM Categoria", conDb))
                {
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        cmbCategoria.Items.Add(new { Id = dr.GetInt32(0), Nombre = dr.GetString(1) });
                    }
                }
            }            

            // Define qué campo mostrar
            cmbCategoria.DisplayMemberPath = "Nombre";
            cmbCategoria.SelectedValuePath = "Id";
        }

        //Parametros para abrir el reporte
        private void btnGenerarReporte_Click(object sender, RoutedEventArgs e)
        {
            //Asignar valores se ingresen o no
            DateTime? fechaInicio = dtpInicio.SelectedDate;
            DateTime? fechaFin = dtpFin.SelectedDate;

            rptRapidPlus rpt = new rptRapidPlus();
            vwRapidPlus vw = new vwRapidPlus();

            // Cargar el reporte
            rpt.Load(@"rptRapidPlus.rpt");

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

            // Verificar categoria y asignar parámetros
            if (cmbCategoria.SelectedItem != null)
            {
                int categoria = (int)(cmbCategoria.SelectedValue);
                rpt.SetParameterValue("@Categoria", categoria);
            }
            else 
            {
                // Si no hay categoría seleccionada
                rpt.SetParameterValue("@Categoria", DBNull.Value); 
            }

            // Asignar el reporte al visor
            vw.crvReportRapidPlus.ViewerCore.ReportSource = rpt;
            vw.Show();

            //Limpiamos los campos
            LimpiarFormulario();
        }
        #endregion



    }
}
