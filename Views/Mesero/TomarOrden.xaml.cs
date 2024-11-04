using Rapid_Plus.Controllers;
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
using System.Windows.Interop;
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

        }

        #region VARIABLES LOCALES
        int numeroMesa = -1;
        int idOrden = -1;
        int idplatillo = -1;
        int idCategoria = -1;
        int idDetalleOrden = -1;
        bool agregando = false, editando = false;
        #endregion

        #region MÉTODOS PERSONALIZADOS
        //Muestra mesas asignadas (ocupadas)
        private void CargarNumeroMesa()
        {
            using (var conDb = new SqlConnection(Properties.Settings.Default.DbRapidPlus))
            {
                conDb.Open();
                using (var command = new SqlCommand("SELECT IdMesa, Mesa FROM Mesa WHERE IdEstado = 0", conDb))
                {
                    SqlDataReader dr = command.ExecuteReader();
                    var mesas = new List<dynamic>();
                    while (dr.Read())
                    {
                        mesas.Add(new { Id = dr.GetInt32(0), Mesa = dr.GetInt32(1) });
                    }

                    cmbMesa.ItemsSource = mesas;
                }
            }

            //Define que campos mostrar
            cmbMesa.DisplayMemberPath = "Mesa";
            cmbMesa.SelectedValuePath = "Id";
        }

        //Muestra categorías de platillos
        private void CargarCategorias()
        {
            using (var conDb = new SqlConnection(Properties.Settings.Default.DbRapidPlus))
            {
                conDb.Open();
                using (var command = new SqlCommand("SELECT IdCategoria, Categoria FROM Categoria", conDb))
                {
                    SqlDataReader dr = command.ExecuteReader();
                    var estados = new List<dynamic>();
                    while (dr.Read())
                    {
                        estados.Add(new { Id = dr.GetInt32(0), Categoria = dr.GetString(1) });
                    }

                    cmbPlatillo.ItemsSource = estados;
                }
            }
            cmbPlatillo.DisplayMemberPath = "Categoria";
            cmbPlatillo.SelectedValuePath = "Id";

        }

        //Limpia objetos 
        private void LimpiarObjetos()
        {
            cmbMesa.SelectedIndex = -1;
            cmbPlatillo.SelectedIndex = -1;
            txtCantidad.Clear();
            txbEstado.Text = null;
            txbOrden.Text = null;
            txbPlatillo.Text = null;
            dgPlatillos.DataContext = null;
            dgOrdenes.DataContext = null;
            dgOrdenes.IsEnabled = false;

        }

        //Obtiene le número de mesa seleccionado en el combobox
        private int NumeroMesa()
        {

            if (cmbMesa.SelectedIndex != -1)
            {
                numeroMesa = (int)cmbMesa.SelectedValue;
            }
            else
            {
                numeroMesa = -1;
            }
            return numeroMesa;
        }

        //Valida campos completos del formulario
        private bool ValidarFomrulario()
        {
            bool estado = true;
            string mensaje = null;

            if (string.IsNullOrEmpty(txbOrden.Text))
            {
                estado = false;
                mensaje += "Número de orden\n";
            }
            if (idplatillo == -1)
            {
                estado = false;
                mensaje += "Platillo\n";
            }
            if (string.IsNullOrEmpty(txtCantidad.Text))
            {
                estado = false;
                mensaje += "Cantidad";
            }

            if (!estado)
            {
                MessageBox.Show("Debe completar los campos:\n" + mensaje, "Validación de formulario", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return estado;
        }

        //Activa o desactiva campos y botones
        private void ControlAcciones()
        {
            bool accion = editando || agregando;

            btnNuevo.IsEnabled = !accion;
            btnEditarOrden.IsEnabled = !accion;
            btnGuardar.IsEnabled = accion;
            btnCancelar.IsEnabled = accion;
            cmbMesa.IsEnabled = accion;
            btnEliminarOrden.IsEnabled = editando; 
            cmbPlatillo.IsEnabled = accion;
            dgOrdenes.IsEnabled = accion;
        }
        #endregion

        #region EVENTOS

        //Cargas por defecto
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LimpiarObjetos();
            CargarNumeroMesa();
            CargarCategorias();
            agregando = false;
            editando = false;

            ControlAcciones();

        }

        //Muestra elementos según mesa seleccionada
        private void cmbMesa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            numeroMesa = NumeroMesa();
            var orden = MeseroController.ObtenerOrdenPorMesa(numeroMesa);
            var ordenes = MeseroController.ListarOrdenesPorMesa(numeroMesa);

            if (orden != null)
            {
                MeseroController.ListarOrdenesPorMesa(numeroMesa);
                dgOrdenes.DataContext = ordenes;

                txbOrden.Text = orden.IdOrden.ToString();
                txbEstado.Text = orden.EstadoOrden;

            }
            else
            {
                txbOrden.Text = string.Empty;
            }


        }

        //Muestra elementos según categoria de platillo seleccionada
        private void cmbPlatillo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPlatillo.SelectedIndex != -1)
            {
                idCategoria = (int)cmbPlatillo.SelectedValue;
                var platillos = MeseroController.ListaPlatillos(idCategoria);
                if (platillos != null)
                {
                    dgPlatillos.DataContext = platillos;

                }
                else
                {
                    MessageBox.Show("No hay Platillos disponibles.");
                }
            }
            
        }

        //Obtiene información de registros seleccionados
        private void dgPlatillos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            OrdenesModel ordenes = (OrdenesModel)dgPlatillos.SelectedItem;
            if (ordenes == null)
            {
                return;
            }
            txbPlatillo.Text = ordenes.NombrePlatillo;
            idplatillo = ordenes.IdPlatillo;

        }
        private void dgOrdenes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OrdenesModel ordenes = (OrdenesModel)dgOrdenes.SelectedItem;
            if (ordenes == null)
            {
                return;
            }
            txtCantidad.Text = ordenes.Cantidad.ToString();
            txbPlatillo.Text = ordenes.Orden;
            idplatillo = ordenes.IdPlatillo;
            idDetalleOrden = ordenes.IdDetalleOrden;
            idOrden = ordenes.IdOrden;
            MessageBox.Show("Detalle de orden: " + idDetalleOrden);
            MessageBox.Show("Orden" + ordenes.IdOrden);
        }

        //BOTONES
        private void btnEditarOrden_Click(object sender, RoutedEventArgs e)
        {
            editando = true;
            agregando = false;
            ControlAcciones();
        }
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Desea cancelar la operación", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                LimpiarObjetos();

                agregando = false;
                editando = false;
                ControlAcciones();
            }
        }
        private void btnAgregarOrden_Click(object sender, RoutedEventArgs e)
        {
            string mensaje = null;
            if (ValidarFomrulario())
            {
                OrdenesModel orden = new OrdenesModel();
                orden.IdOrden = Convert.ToInt32(txbOrden.Text);
                orden.Cantidad = Convert.ToInt32(txtCantidad.Text);
                orden.IdEstado = 1;
                orden.IdPlatillo = idplatillo;

                

                if (agregando)
                {
                    idOrden = MeseroController.InsertarOrden(orden);
                    mensaje = "Orden creada con éxito";
                }
                else
                {
                    idOrden = MeseroController.EditarOrden(orden,idOrden);
                    mensaje = "La orden fue actualizada con éxito";
                }

                if (idOrden > 0)
                {

                    MessageBox.Show(mensaje, "Validación de formulario", MessageBoxButton.OK, MessageBoxImage.Information);
                    agregando = false;
                    editando = false;
                    ControlAcciones();
                }
                LimpiarObjetos();

            }


        }
        private void btnEliminarOrden_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Desea eliminar el detalle de la orden?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (idDetalleOrden > 0 && idOrden > 0)
                {
                    MeseroController.EliminarDetalleOrden(idDetalleOrden, idOrden);
                    MessageBox.Show("Detalle de orden eliminado con éxito.", "Confirmación", MessageBoxButton.OK, MessageBoxImage.Information);

                    LimpiarObjetos();
                }
                else
                {
                    MessageBox.Show("Seleccione un detalle de orden válido para eliminar.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            agregando = true;
            editando = false;

            ControlAcciones();
        }

        #endregion


    }
}
