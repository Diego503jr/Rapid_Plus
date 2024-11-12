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
using System.Windows.Threading;

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
            IniciarTemporizador();
        }

        #region DECLARACION DE VARIABLES LOCALES
        private DispatcherTimer timer;
        int IdEstadoOrden = -1;
        int idMesa = -1;
        #endregion
        #region MÉTODOS PERSONALIZADOS
        private int Mesa()
        {

            if (cmbNumMesa.SelectedIndex != -1)
            {
                idMesa = (int)cmbNumMesa.SelectedValue;
            }
            else
            {
                idMesa = -1;
            }
            return idMesa;
        }

        private void MostrarOrdenesPorMesa()
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

                    cmbNumMesa.ItemsSource = mesas;
                }
            }

            //Define que campos mostrar
            cmbNumMesa.DisplayMemberPath = "Mesa";
            cmbNumMesa.SelectedValuePath = "IdMesa";
        }
        private void EditarEstadoOrden()
        {
            if (!ValidarFomrulario())
            {
                MessageBox.Show("Por favor completa todos los campos requeridos.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Obtener el ID de la orden desde la interfaz de usuario
                if (!int.TryParse(txbOrden.Text, out int idOrden))
                {
                    return;
                }

                // Asignar el estado 
                int nuevoEstado = 1;

                OrdenesModel estado = new OrdenesModel
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
            MostrarOrdenesPorMesa();
        }
        private bool ValidarFomrulario()
        {
            bool estado = true;
            string mensaje = null;

            if (string.IsNullOrEmpty(cmbNumMesa.Text))
            {
                estado = false;
                mensaje += "Número de mesa\n";
            }

            if (string.IsNullOrEmpty(txbOrden.Text))
            {
                estado = false;
                mensaje += "Orden\n";
            }

            if (string.IsNullOrEmpty(txbEstado.Text))
            {
                estado = false;
                mensaje += "Estado de la orden\n";
            }

            if (!estado)
            {
                MessageBox.Show("Debe completar los campos:\n" + mensaje, "Validación de formulario", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return estado;
        }
        void LimpiarFormulario()
        {
            cmbNumMesa.SelectedIndex = -1;
            dgOrdenes.DataContext = null;
            txbOrden.Text = string.Empty; // Limpia el contenido del TextBlock
            txbEstado.Text = string.Empty; // Limpia el contenido del TextBox
        }
        #endregion

        #region EVENTOS
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MostrarOrdenesPorMesa();
        }
        private void IniciarTemporizador()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += Timer_Tik;
            timer.Start();
        }
        private void Timer_Tik(object sender, EventArgs e)
        {
            MostrarOrdenesPorMesa();
            MostrarOrden();
        }
        private void btnLista_Click(object sender, RoutedEventArgs e)
        {
            //Validamos si se quiere cambiar el estado
            if (!ValidarFomrulario())
            {
                MessageBox.Show("Por favor completa todos los campos requeridos.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else 
            {
                MessageBoxResult resultado = MessageBox.Show(
                    "¿Desea cambiar el estado de la orden?",
                    "Confirmación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );
                if (resultado == MessageBoxResult.Yes) 
                {
                    EditarEstadoOrden();
                }
            }
        }

        private void MostrarOrden() 
        {
            idMesa = Mesa();
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

        private void cmbNumMesa_DropDownOpened(object sender, EventArgs e)
        {
            if (cmbNumMesa.Items.Count == 0)
            {
                MessageBox.Show("No hay mesas con ordenes pendientes", "Mesas", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
          
        }

        private void cmbNumMesa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MostrarOrden();
        }
        #endregion

    }

}
