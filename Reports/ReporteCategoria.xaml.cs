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
    /// Lógica de interacción para ReporteCategoria.xaml
    /// </summary>
    public partial class ReporteCategoria : Window
    {
        public ReporteCategoria()
        {
            InitializeComponent();
            CargarCategoria();
        }

        //Conexion a la DB
        SqlConnection conDBRP = new SqlConnection(Properties.Settings.Default.DbRapidPlus);

        void LimpiarFormulario()
        {
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
        private void btnGnRCategoria_Click(object sender, RoutedEventArgs e)
        {

            rptCategoria rpt = new rptCategoria();
            vwRapidPlus vw = new vwRapidPlus();

            // Cargar el reporte
            rpt.Load(@"rptCategoria.rpt");


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
            vw.ShowDialog();

            //Limpiamos los campos
            LimpiarFormulario();
        }
    }
}
