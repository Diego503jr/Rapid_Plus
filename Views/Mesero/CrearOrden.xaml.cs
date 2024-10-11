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
    /// Lógica de interacción para CrearOrden.xaml
    /// </summary>
    public partial class CrearOrden : Page
    {
        public CrearOrden()
        {
            InitializeComponent();
            CargarNumeroMesa();
        }

        private void CargarNumeroMesa()
        {
            using (var conDb = new SqlConnection(Properties.Settings.Default.DbRapidPlus))
            {
                conDb.Open();
                using (var command = new SqlCommand("SELECT Id, Mesa FROM Mesas WHERE Id_Estado = 1", conDb))
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cmbMesa.SelectedIndex = -1;
            txbOrden.Text = string.Empty;
            txtApellido.Clear();
            txtNombre.Clear();
        }
    }
}
