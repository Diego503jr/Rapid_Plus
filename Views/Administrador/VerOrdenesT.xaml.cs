using Rapid_Plus.Controllers;
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

namespace Rapid_Plus.Views.Administrador
{
    /// <summary>
    /// Lógica de interacción para VerOrdenesT.xaml
    /// </summary>
    public partial class VerOrdenesT : Page
    {
        public VerOrdenesT()
        {
            InitializeComponent();
        }

        #region Declaración de Variables Locales
        int IdEstadoOrden = -1;
        #endregion

        #region Metodos Personalizados
        void MostrarOrdenes()
        {
            dgOrdenes.DataContext = OrdenController.MostrarOrdenes();
        }

        private void CargarEstado()
        {
            using (var conDb = new SqlConnection(Properties.Settings.Default.DbRapidPlus))
            {
                conDb.Open();
                using (var command = new SqlCommand("SELECT IdEstadoOrden, EstadoOrden FROM EstadoOrden WHERE IdEstadoOrden != 3", conDb))
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

            //Define que campos mostrar
            cmbFiltro.DisplayMemberPath = "EstadoOrden";
            cmbFiltro.SelectedValuePath = "IdEstadoOrden";

        }


        #endregion

        #region Eventos
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
                var ordenes = OrdenController.MostrarOrdenes(IdEstadoOrden);
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
        #endregion

    }
}
