using Rapid_Plus.Controllers;
using Rapid_Plus.Models.Mesero;
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
        int idMesa = -1;
        #endregion

        #region MÉTODOS PERSONALIZADOS
        //Lista las ordenes en un datagrid
        private int IdMesa()
        {

            if (cmbFiltro.SelectedIndex != -1)
            {
                idMesa = (int)cmbFiltro.SelectedValue;
            }
            else
            {
                idMesa = -1;
            }
            return idMesa;
        }

        //Llena el combobox con los estados Listo y pendiente
        private void CargarNumeroMesa()
        {
            using (var conDb = new SqlConnection(Properties.Settings.Default.DbRapidPlus))
            {
                conDb.Open();
                using (var command = new SqlCommand("SPMOSTRARMESASPENDIENTES", conDb))
                {
                    SqlDataReader dr = command.ExecuteReader();
                    var mesas = new List<dynamic>();
                    while (dr.Read())
                    {
                        mesas.Add(new { IdMesa = dr.GetInt32(0), Mesa = dr.GetInt32(1) });
                    }

                    cmbFiltro.ItemsSource = mesas;
                }
            }

            //Define que campos mostrar
            cmbFiltro.DisplayMemberPath = "Mesa";
            cmbFiltro.SelectedValuePath = "IdMesa";
        }

        private void LimpiarObjetos()
        {
            cmbFiltro.SelectedIndex = -1;
            dgOrdenes.DataContext = null;
            txbOrden.Text = null;
            txbEstado.Text = null;
        }
        #endregion

        #region EVENTOS

        //Carga por defecto
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LimpiarObjetos();
            CargarNumeroMesa();
        }

        //Filtra ordenes según el estado
        private void cmbFiltro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            idMesa = IdMesa();
            var detalle = DetalleOrdenController.ObtenerDetalleOrden(idMesa);
            var ordenes = OrdenController.MostrarOrdenesPorMesa(idMesa);

            if (detalle != null)
            {
                OrdenController.MostrarOrdenesPorMesa(idMesa);
                dgOrdenes.DataContext = ordenes;

                txbOrden.Text = detalle.IdOrden.ToString();
                txbEstado.Text = detalle.EstadoOrden;
            }
            else
            {
                txbOrden.Text = string.Empty;
            }


        }

        private void btnLimpiarFiltro_Click(object sender, RoutedEventArgs e)
        {
            LimpiarObjetos();
        }

    }
    #endregion


}
