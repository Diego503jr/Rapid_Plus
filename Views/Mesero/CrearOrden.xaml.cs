using Rapid_Plus.Controllers.Mesero;
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
    /// Lógica de interacción para CrearOrden.xaml
    /// </summary>
    public partial class CrearOrden : Page
    {
        public CrearOrden()
        {
            InitializeComponent();
            CargarNumeroMesa();
        }
        #region Declaracion de variables locales
        private DateTime fecha = DateTime.Now;
        private int idOrden = 0;

        #endregion


        //Llenar combobox de numero de mesa
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


        //Validad campos vacios 
        private bool ValidarFomrulario()
        {
            bool estado = true;
            string mensaje = null;

            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                estado = false;
                mensaje += "Nombre de cliente\n";
            }
            if (string.IsNullOrEmpty(txtApellido.Text))
            {
                estado = false;
                mensaje += "Apellido de cliente\n";
            }
            if (string.IsNullOrEmpty(cmbMesa.Text))
            {
                estado = false;
                mensaje += "Número de mesa";
            }

            if (!estado)
            {
                MessageBox.Show("Debe completar los campos:\n" + mensaje, "Validación de formulario", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return estado;
        }

        private void LimpiarObjetos()
        {
            cmbMesa.SelectedIndex = -1;
            txtApellido.Clear();
            txtNombre.Clear();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LimpiarObjetos();
        }

        private void btnCrearOrden_Click(object sender, RoutedEventArgs e)
        {
            string mensaje = null;
            if (ValidarFomrulario())
            {
                OrdenesModel orden = new OrdenesModel();
                orden.NombreCliente = txtNombre.Text;
                orden.ApellidoCliente = txtApellido.Text;
                orden.UsuarioId = 0;
                orden.FechaOrden = fecha;
                orden.Total = 0;
                orden.Mesa = (int)cmbMesa.SelectedValue;
                orden.IdEstadoOrden = 0;

                idOrden = MeseroController.CrearOrden(orden);
                mensaje = "Orden creada con éxito";

                if(idOrden > 0)
                {
                    LimpiarObjetos();
                    MessageBox.Show(mensaje, "Validación de formulario", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            
        }

    }
}
