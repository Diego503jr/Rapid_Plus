using MaterialDesignColors;
using Rapid_Plus.Controllers;
using Rapid_Plus.Models;
using Rapid_Plus.Models.Mesero;
using Rapid_Plus.Views.Mesero;
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

namespace Rapid_Plus.Views.JefeDeCocina
{
    /// <summary>
    /// Lógica de interacción para EstadoOrden.xaml
    /// </summary>
    public partial class EstadoOrden : Page
    {
        public EstadoOrden()
        {
            InitializeComponent();
        }

        #region DECLARACION DE VARIABLES LOCALES
        int IdEstadoOrden = -1;
        #endregion
        #region MÉTODOS PERSONALIZADOS
        void MostrarOrdenes()
        {
            dgOrdenes.DataContext = JefeCocinaController.VerOrdenes();
        }

        private void MostrarEstado()
        {
            using (var conDb = new SqlConnection(Properties.Settings.Default.DbRapidPlus))
            {
                conDb.Open();
                using (var command = new SqlCommand("SELECT IdEstadoOrden, EstadoOrden FROM EstadoOrden WHERE IdEstadoOrden = 0", conDb))
                {
                    SqlDataReader dr = command.ExecuteReader();
                    var estados = new List<dynamic>();
                    while (dr.Read())
                    {
                        estados.Add(new { IdEstadoOrden = dr.GetInt32(0), EstadoOrden = dr.GetString(1) });
                    }

                }
            }
        }

        private bool ValidarFomrulario()
        {
            bool estado = true;
            string mensaje = null;

            if (string.IsNullOrEmpty(txtOrden.Text))
            {
                estado = false;
                mensaje += "Orden\n";
            }

            if (!estado)
            {
                MessageBox.Show("Debe completar los campos:\n" + mensaje, "Validación de formulario", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return estado;
        }
        void LimpiarFormulario()
        {
            txbIdOrden.Text = string.Empty; // Limpia el contenido del TextBlock
            txtOrden.Text = string.Empty; // Limpia el contenido del TextBox
        }
        #endregion
        #region EVENTOS
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MostrarOrdenes();
            MostrarEstado();
        }
        private void dgOrdenes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EstadoModel estado = (EstadoModel)dgOrdenes.SelectedItem;
            if (estado == null)
            {
                return;
            }
            txbIdOrden.Text = estado.IdOrden.ToString();
            txtOrden.Text = estado.Orden.ToString();
        }

        private void btnLista_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidarFomrulario())
            {
                MessageBox.Show("Por favor completa todos los campos requeridos.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Obtener el ID de la orden desde la interfaz de usuario
                if (!int.TryParse(txbIdOrden.Text, out int idOrden))
                {
                    return;
                }

                // Asignar el estado 
                int nuevoEstado = 1;

                EstadoModel estado = new EstadoModel
                {
                    IdEstadoOrden = nuevoEstado
                };

                // Llamar al método para editar el estado de la orden
                int resultado = JefeCocinaController.EditarEstadoOrden(estado, idOrden);

                if (resultado == 1)
                {
                    MessageBox.Show("El estado de la orden ha sido actualizado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar el estado de la orden.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LimpiarFormulario();
            MostrarOrdenes();
        }
        #endregion
    }



}
