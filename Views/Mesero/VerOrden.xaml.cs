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
           
        }

        #region DECLARACION DE VARIABLES LOCALES
        int IdEstadoOrden = -1;
        #endregion

        #region MÉTODOS PERSONALIZADOS
        void MostrarOrdenes()
        {
            dgOrdenes.DataContext = MeseroController.ListarOrdenes();
        }

        private void CargarEstado()
        {
            using (var conDb = new SqlConnection(Properties.Settings.Default.DbRapidPlus))
            {
                conDb.Open();
                using (var command = new SqlCommand("SELECT IdEstadoOrden, EstadoOrden FROM EstadoOrden WHERE IdEstadoOrden != 2", conDb))
                {
                    SqlDataReader dr = command.ExecuteReader();
                    var estados = new List<dynamic>();
                    while (dr.Read())
                    {
                        estados.Add(new { IdEstadoOrden = dr.GetInt32(0), EstadoOrden = dr.GetString(1) });
                    }

                    cmbFiltro.ItemsSource = estados;
                }
            }
            cmbFiltro.DisplayMemberPath = "EstadoOrden";
            cmbFiltro.SelectedValuePath = "IdEstadoOrden";

        }
        #endregion

        #region EVENTOS
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MostrarOrdenes();
            cmbFiltro.SelectedIndex = -1;
            CargarEstado();
        }
        private void cmbFiltro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFiltro.SelectedIndex != -1)
            {
                IdEstadoOrden = (int)cmbFiltro.SelectedValue;
                var ordenes = MeseroController.ListarOrdenes(IdEstadoOrden);
                if (ordenes != null)
                {
                    dgOrdenes.DataContext = ordenes;

                }
                else
                {
                    MessageBox.Show("No hay Ordenes disponibles.");
                }
            }

           
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            cmbFiltro.SelectedIndex = -1;
            MostrarOrdenes();
        }
    }
    #endregion


}
