using Rapid_Plus.Controllers.Mesero;
using Rapid_Plus.Models;
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
    /// Lógica de interacción para TomarOrden.xaml
    /// </summary>
    public partial class TomarOrden : Page
    {
        public TomarOrden()
        {
            InitializeComponent();
            CargarNumeroMesa();

        }
        private void CargarNumeroMesa()
        {
            using (var conDb = new SqlConnection(Properties.Settings.Default.DbRapidPlus))
            {
                conDb.Open();
                using (var command = new SqlCommand("SELECT Id, Mesa FROM Mesas WHERE Id_Estado = 1", conDb)) //Muestra unicamente las disponibles
                {
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        cmbMesa.Items.Add(new { Id = dr.GetInt32(0), Nombre = dr.GetInt32(1) });
                    }
                }
            }

            // Define qué campo mostrar
            cmbMesa.DisplayMemberPath = "Nombre";
            cmbMesa.SelectedValuePath = "Id";
        }
        private void LimpiarObjetos()
        {
            cmbMesa.SelectedIndex = -1;
        }

        private int NumeroMesa()
        {
            int numeroMesa = -1;
            if (cmbMesa.SelectedIndex != -1)
            {
                // Asume que el valor seleccionado es un entero
                 numeroMesa = (int)cmbMesa.SelectedItem;

            }
            return numeroMesa;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LimpiarObjetos();
        }

        private void cmbMesa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
